﻿using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using net_core_backend.Context;
using net_core_backend.Helpers;
using net_core_backend.Models;
using net_core_backend.Services.Interfaces;
using net_core_backend.Services.Extensions;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BC = BCrypt.Net.BCrypt;
using System.Security.Cryptography;

namespace net_core_backend.Services
{
    public class AccountService : DataService<DefaultModel>, IAccountService
    {
        private readonly IContextFactory contextFactory;
        private readonly IHttpContextAccessor httpContext;
        private readonly AppSettings appSettings;
        public AccountService(IContextFactory _contextFactory, IOptions<AppSettings> appSettings, IHttpContextAccessor httpContext) : base(_contextFactory)
        {
            contextFactory = _contextFactory;
            this.httpContext = httpContext;
            this.appSettings = appSettings.Value;
        }


        public async Task<Users> GetUserDetailsJWT(int id)
        {
            if (id == 0) throw new ArgumentException("There is no ID in the JWT Token");

            using (var a = contextFactory.CreateDbContext())
            {
                return await a.Users.Where(x => x.Id == id).FirstOrDefaultAsync();
            }
        }

        public async Task<VerificationResponse> Login(LoginRequest model, string ipAddress = null)
        {
            using var a = contextFactory.CreateDbContext();

            var user = await a.Users.FirstOrDefaultAsync(x => x.Email == model.Email);

            if (user == null)
            {
                throw new ArgumentException("This email isn't registered in our system");
            }

            if (!BC.Verify(model.Password, user.Password))
            {
                throw new ArgumentException("Invalid password");
            }

            // authentication successful so generate jwt token
            var token = GenerateJwtToken(user);
            var refreshToken = GenerateRefreshToken(ipAddress);

            var activeRF = await a.RefreshTokens.Where(x => x.UserId == user.Id && x.RevokedAt == null).ToListAsync();
            activeRF.ForEach(x =>
            {
                x.RevokedAt = DateTime.UtcNow;
                x.RevokedByIp = ipAddress;
                x.ReplacedByToken = refreshToken.Token;
            });


            user.RefreshTokens.Add(refreshToken);
            a.Update(user);
            await a.SaveChangesAsync();

            return new VerificationResponse(user, token, refreshToken.Token);
        }

        public async Task<VerificationResponse> Register(AddUserRequest requestInfo, string ipAddress = null)
        {
            using (var db = contextFactory.CreateDbContext())
            {
                var user = await db.Users.FirstOrDefaultAsync(u => u.Email == requestInfo.Email);

                if (user != null)
                {
                    user.FirstName = requestInfo.FirstName;
                    user.LastName = requestInfo.LastName;
                    user.Password = BC.HashPassword(requestInfo.Password);

                    db.Update(user);
                    await db.SaveChangesAsync();
                    
                    var token = GenerateJwtToken(user);
                    var refreshToken = GenerateRefreshToken(ipAddress);

                    user.RefreshTokens.Add(refreshToken);

                    db.Update(user);
                    await db.SaveChangesAsync();
                    
                    return new VerificationResponse(user, token, refreshToken.Token);
                }

                throw new ArgumentException("No user found");

            }
        }
        
        public async Task<VerificationResponse> RefreshToken(string token, string ipaddress)
        {
            using var a = contextFactory.CreateDbContext();

            var rToken = await a.RefreshTokens.Include(x => x.User).FirstOrDefaultAsync(x => x.Token == token);
            if (rToken == null || !rToken.IsActive) return null;

            var newRefreshToken = GenerateRefreshToken(ipaddress);
            rToken.RevokedAt = DateTime.UtcNow;
            rToken.RevokedByIp = ipaddress;
            rToken.ReplacedByToken = newRefreshToken.Token;

            rToken.User.RefreshTokens.Add(newRefreshToken);

            a.Update(rToken.User);
            await a.SaveChangesAsync();


            var jwtToken = GenerateJwtToken(rToken.User);

            return new VerificationResponse(rToken.User, jwtToken, newRefreshToken.Token);
        }

        public async Task<bool> RevokeToken(string token, string ipAddress)
        {
            using var a = contextFactory.CreateDbContext();

            var rfToken = await a.RefreshTokens.FirstOrDefaultAsync(x => x.Token == token);

            // return false if no user found with token
            if (rfToken == null) return false;

            // return false if token is not active
            if (!rfToken.IsActive) return false;

            // revoke token and save
            rfToken.RevokedAt = DateTime.UtcNow;
            rfToken.RevokedByIp = ipAddress;
            a.Update(rfToken);
            await a.SaveChangesAsync();

            return true;
        }

        public async Task CreateAdmin(AddUserRequest requestInfo)
        {
            using (var db = contextFactory.CreateDbContext())
            {

                if (await db.Users.FirstOrDefaultAsync(u => u.Email == requestInfo.Email) == null)
                {
                    var user = new Users
                    {
                        FirstName = requestInfo.FirstName,
                        LastName = requestInfo.LastName,
                        Email = requestInfo.Email,
                        Password = BC.HashPassword(requestInfo.Password),
                        Role = "Admin"

                    };

                    await db.AddAsync(user);
                    await db.SaveChangesAsync();
                    return;
                    
                }
                throw new ArgumentException("Email already used");
           
            }

        }
        
        private string GenerateJwtToken(Users user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role),

                }),
                Expires = DateTime.UtcNow.AddHours(24),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                // Should add issues and audience but needs testing with production
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private RefreshTokens GenerateRefreshToken(string ipAddress)
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            
            var randomBytes = new byte[64];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            return new RefreshTokens
            {
                Token = Convert.ToBase64String(randomBytes),
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow,
                CreatedByIp = ipAddress
            };
        }
    }
}
