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
        private readonly AppSettings appSettings;
        public AccessTokenService(IContextFactory _contextFactory, IOptions<AppSettings> appSettings, IHttpContextAccessor httpContext) : base(_contextFactory)
        {
            contextFactory = _contextFactory;
            this.httpContext = httpContext;
            this.appSettings = appSettings.Value;
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

                return new CreateAccessTokenResponse(user, newAccessToken);
            }
        }
    }
}
