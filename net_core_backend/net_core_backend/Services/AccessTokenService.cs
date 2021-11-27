using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using net_core_backend.Context;
using net_core_backend.Helpers;
using net_core_backend.Models;
using net_core_backend.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using net_core_backend.Services.Extensions;

namespace net_core_backend.Services
{
    public class AccessTokenService : DataService<DefaultModel>, IAccessTokenService
    {
        private readonly IContextFactory contextFactory;
        private readonly IHttpContextAccessor httpContext;
        public AccessTokenService(IContextFactory _contextFactory, IHttpContextAccessor httpContext) : base(_contextFactory)
        {
            contextFactory = _contextFactory;
            this.httpContext = httpContext;
        }

        public async Task<CreateAccessTokenResponse> CreateAccessToken()
        {
            using (var db = contextFactory.CreateDbContext())
            {
                int userId = httpContext.GetCurrentUserId();
                var user = await db.Users.FirstOrDefaultAsync(u => u.Id.Equals(userId));

                if (user == null)
                {
                    throw new ArgumentException("A user with these credentials does not exist");
                }

                var at = Guid.NewGuid().ToString("N");

                while (await db.AccessTokens.FirstOrDefaultAsync(token => token.AccessToken.Equals(at)) != null)
                {
                    at = Guid.NewGuid().ToString("N");
                }

                var newAccessToken = new AccessTokens(at);
                user.AccessTokens.Add(newAccessToken);
                db.Update(user);
                await db.SaveChangesAsync();

                return new CreateAccessTokenResponse(newAccessToken);
            }
        }

        public async Task CheckAccessToken(string accessToken)
        {
            //check if the accesstoken given is in our accesstokens database, if not throw exception
            using (var db = contextFactory.CreateDbContext())
            {
                var Token = await db.AccessTokens.FirstOrDefaultAsync(a => a.AccessToken == accessToken);
                if (Token == null || Token.Active == false)
                {
                    throw new ArgumentException("Incorrect accesstoken, request denied.");
                }
            }
        }

        public async Task ToggleAccessToken(int accessTokenId)
        {
            using (var db = contextFactory.CreateDbContext())
            {
                var token = await db.AccessTokens.FirstOrDefaultAsync(a => a.Id == accessTokenId);
                if (token == null)
                {
                    throw new ArgumentException("no accessToken found with given id");
                }
                token.Active = !token.Active;
                db.Update(token);
                await db.SaveChangesAsync();
            }
        }
    }
}
