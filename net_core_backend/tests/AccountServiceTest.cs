using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using net_core_backend.Context;
using net_core_backend.Helpers;
using net_core_backend.Models;
using net_core_backend.Services;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Xunit;
using BC = BCrypt.Net.BCrypt;

namespace tests
{
    public class AccountServiceTest
    {
        private readonly IDbContextFactory<OneBlinqDBContext> testContextFactory;
        private AccountService sut;
        public AccountServiceTest()
        {
            // Mock IHttpContextAccessor
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var httpContext = new DefaultHttpContext();

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, "1"),
            };

            mockHttpContextAccessor.Setup(h => h.HttpContext.User.Claims).Returns(claims);

            // Mocking database
            testContextFactory = new TestContextFactory();

            // Mocking appSettings
            var appSettings = new AppSettings
            {
                Secret = "1234567890abcdefghryjdghfVYDDDAB"
            };
            var mockAppSettings = Options.Create(appSettings);

            // Mocking IMailingService 
            var mockMailingService = new Mock<IMailingService>();

            sut = new AccountService(testContextFactory, mockAppSettings, mockHttpContextAccessor.Object, mockMailingService.Object);
        }

        [Fact]
        public async Task CreateAdminTest()
        {
            AddUserRequest model = new AddUserRequest
            {
                FirstName = "First",
                LastName = "Last",
                Email = "Email",
                Password = "password"

            };
            //await sut.CreateAdmin(model);
            Should.NotThrow(sut.CreateAdmin(model));

            using var db = testContextFactory.CreateDbContext();

            var user = await db.Users.FirstOrDefaultAsync((u => u.Email == model.Email));

            Assert.NotNull(user);
            Should.Equals(user.Role, "Admin");
        }

        [Fact]
        public async Task ChangePasswordTest()
        {
            ChangePasswordRequest model = new ChangePasswordRequest
            {
                CurrentPassword = "Current password",
                NewPassword = "New passworD"
            };

            using var db = testContextFactory.CreateDbContext();
            var user = new Users
            {
                Email = "email",
                Password = BC.HashPassword("Current password")
            };
            await db.Users.AddAsync(user);
            await db.SaveChangesAsync();

            await sut.ChangePassword(model);

            // Reload method updates the database you are using with the up-to-date data
            db.Entry(user).Reload();

            var user2 = await db.Users.FirstOrDefaultAsync((u => u.Email == user.Email));

            BC.Verify(model.NewPassword, user2.Password).ShouldBeTrue();
        }

        [Fact]
        public async Task RegisterTest()
        {
            using var db = testContextFactory.CreateDbContext();

            var user = new Users()
            {
                Email = "someDudesEmail@gmail.com",
                Role = "User"
            };

            await db.AddAsync(user);
            await db.SaveChangesAsync();


            var verifyResponse = await sut.Register(new AddUserRequest()
            {
                Email = user.Email,
                Password = "MyPassword",
                FirstName = "Sam",
                LastName = "Sammer"
            });

            verifyResponse.RefreshToken.ShouldNotBeNullOrEmpty();
            verifyResponse.Token.ShouldNotBeNullOrEmpty();

            verifyResponse.Email.ShouldBe(user.Email);
        }

        [Fact]
        public async Task ExistingUserRegisterTest()
        {
            using var db = testContextFactory.CreateDbContext();

            var user = new Users()
            {
                Email = "someDudesEmail@gmail.com",
                Role = "User",
                Password = "SomeOtherPassword"
            };

            await db.AddAsync(user);
            await db.SaveChangesAsync();

            await Should.ThrowAsync<ArgumentException>(() =>
                sut.Register(new AddUserRequest()
                {
                    Email = user.Email,
                    Password = "MyPassword",
                    FirstName = "Sam",
                    LastName = "Sammer"
                }));
        }
        [Fact]
        public async Task LoginTest()
        {
            using var db = testContextFactory.CreateDbContext();


            var user = new Users()
            {
                Email = "someDudesEmail@gmail.com",
                Role = "User",
                Password = BC.HashPassword("SomeOtherPassword")
            };

            await db.AddAsync(user);
            await db.SaveChangesAsync();

            var response = await sut.Login(new LoginRequest()
            {
                Email = user.Email,
                Password = "SomeOtherPassword"
            });

            response.RefreshToken.ShouldNotBeNullOrEmpty();
            response.Token.ShouldNotBeNullOrEmpty();

            response.Email.ShouldBe(user.Email);
        }

        [Fact]
        public async Task LoginNotSamePasswordTest()
        {
            using var db = testContextFactory.CreateDbContext();


            var user = new Users()
            {
                Email = "someDudesEmail@gmail.com",
                Role = "User",
                Password = BC.HashPassword("SomeOtherPassword")
            };

            await db.AddAsync(user);
            await db.SaveChangesAsync();

            await Should.ThrowAsync<ArgumentException>(() => sut.Login(new LoginRequest()
            {
                Email = user.Email,
                Password = "WrongPassword"
            }));
        }

        [Fact]
        public async Task RenewRefreshTokenTest()
        {
            using var db = testContextFactory.CreateDbContext();

            var user = new Users()
            {
                Email = "someDudesEmail@gmail.com",
                Role = "User",
                Password = BC.HashPassword("SomeOtherPassword")
            };
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();

            var randomBytes = new byte[64];
            rngCryptoServiceProvider.GetBytes(randomBytes);

            user.RefreshTokens.Add(new RefreshTokens()
            {
                Active = true,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddMinutes(15),
                Token = Convert.ToBase64String(randomBytes)
            });

            await db.AddAsync(user);
            await db.SaveChangesAsync();

            var response = await 
                sut.RefreshToken(Convert.ToBase64String(randomBytes), null);

            response.RefreshToken.ShouldNotBeNullOrEmpty();
            response.Token.ShouldNotBeNullOrEmpty();

            response.Email.ShouldBe(user.Email);
        }

        [Fact]
        public async Task ExpiredRefreshTokenTest()
        {
            using var db = testContextFactory.CreateDbContext();

            var user = new Users()
            {
                Email = "someDudesEmail@gmail.com",
                Role = "User",
                Password = BC.HashPassword("SomeOtherPassword")
            };
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();

            var randomBytes = new byte[64];
            rngCryptoServiceProvider.GetBytes(randomBytes);

            user.RefreshTokens.Add(new RefreshTokens()
            {
                Active = true,
                CreatedAt = DateTime.UtcNow.AddDays(-5),
                ExpiresAt = DateTime.UtcNow.AddMinutes(-15),
                Token = Convert.ToBase64String(randomBytes)
            });

            await db.AddAsync(user);
            await db.SaveChangesAsync();

            var response = await
                sut.RefreshToken(Convert.ToBase64String(randomBytes), null);

            response.ShouldBe(null);
        }

        [Fact]
        public async Task RevokeTokenTest()
        {
            using var db = testContextFactory.CreateDbContext();

            var user = new Users()
            {
                Email = "someDudesEmail@gmail.com",
                Role = "User",
                Password = BC.HashPassword("SomeOtherPassword")
            };
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();

            var randomBytes = new byte[64];
            rngCryptoServiceProvider.GetBytes(randomBytes);

            user.RefreshTokens.Add(new RefreshTokens()
            {
                Active = true,
                CreatedAt = DateTime.UtcNow.AddDays(5),
                ExpiresAt = DateTime.UtcNow.AddMinutes(15),
                Token = Convert.ToBase64String(randomBytes)
            });

            await db.AddAsync(user);
            await db.SaveChangesAsync();

            var loginResponse = await sut.Login(new LoginRequest()
            {
                Email = user.Email,
                Password = "SomeOtherPassword"
            });

            var response = await sut.RevokeCookie(loginResponse.RefreshToken, null);
            var failedResponse = await sut.RevokeCookie(Convert.ToBase64String(randomBytes), null);

            response.ShouldBe(true);
            failedResponse.ShouldBe(false);
        }

        [Fact]
        public async Task ForgottenPasswordRequestTest()
        {
            using var db = testContextFactory.CreateDbContext();
            var user = new Users()
            {
                Email = "someDudesEmail@gmail.com",
                Role = "User",
                Password = BC.HashPassword("SomeOtherPassword")
            };

            await db.AddAsync(user);
            await db.SaveChangesAsync();

            await sut.ForgottenPasswordRequest(user.Email);

            db.ForgottenPasswordTokens.Count().ShouldBe(1);
        }

        [Fact]
        public async Task ForgottenPasswordVerifyRequestTest()
        {
            using var db = testContextFactory.CreateDbContext();
            var user = new Users()
            {
                Email = "someDudesEmail@gmail.com",
                Role = "User",
                Password = BC.HashPassword("SomeOtherPassword")
            };

            await db.AddAsync(user);
            await db.SaveChangesAsync();

            await sut.ForgottenPasswordRequest(user.Email);


            var response = await sut.ForgottenPasswordVerification(new ForgottenPasswordVerificationRequest()
            {
                Token = db.ForgottenPasswordTokens.First().Token,
                NewPassword = "MyNewPassword"
            });

            response.Token.ShouldNotBeNullOrEmpty();
            response.Email.ShouldBe(user.Email);

        }

        [Fact]
        public async Task ForgottenPasswordExpiredVerifyRequestTest()
        {
            using var db = testContextFactory.CreateDbContext();
            var user = new Users()
            {
                Email = "someDudesEmail@gmail.com",
                Role = "User",
                Password = BC.HashPassword("SomeOtherPassword")
            };

            await db.AddAsync(user);
            await db.SaveChangesAsync();

            await sut.ForgottenPasswordRequest(user.Email);
            db.ForgottenPasswordTokens.First().ExpiresAt = DateTime.UtcNow.AddMinutes(-15);
            await db.SaveChangesAsync();

            await Should.ThrowAsync<ArgumentException>(() =>

            sut.ForgottenPasswordVerification(new ForgottenPasswordVerificationRequest()
            {
                Token = db.ForgottenPasswordTokens.First().Token,
                NewPassword = "MyNewPassword"
            })
            );
        }
    }
}
