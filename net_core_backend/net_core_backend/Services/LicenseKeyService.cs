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

namespace net_core_backend.Services
{
    public class LicenseKeyService : DataService<DefaultModel>, ILicenseKeyService
    {
        private readonly IContextFactory contextFactory;
        private readonly IHttpContextAccessor httpContext;
        private readonly AppSettings appSettings;
        private AccessTokenService accessTokenService;
        public LicenseKeyService(IContextFactory _contextFactory, IOptions<AppSettings> appSettings, IHttpContextAccessor httpContext) : base(_contextFactory)
        {
            contextFactory = _contextFactory;
            this.httpContext = httpContext;
            this.appSettings = appSettings.Value;
            accessTokenService = new AccessTokenService(_contextFactory, appSettings, httpContext);
        }

        public async Task<VerifyLicenseResponse> VerifyLicense(VerifyLicenseRequest model)
        {
            using (var db = contextFactory.CreateDbContext())
            {
                try
                {
                    var accessToken = await db.AccessTokens.FirstOrDefaultAsync(at => at.AccessToken.Equals(model.AccessToken));

                    if (accessToken == null)
                    {
                        throw new ArgumentException("Provided access token does not exist");
                    }
                    var user = await db.Users.FirstOrDefaultAsync(u => u.Email.Equals(model.Email));
                    if (user == null)
                    {
                        throw new ArgumentException("A user with these credentials does not exist");
                    }

                    var license = await db.Licenses.FirstOrDefaultAsync(l => l.UserId.Equals(user.Id) && l.LicenseKey.Equals(model.LicenseKey));

                    if (license == null)
                    {
                        throw new ArgumentException("The user does not have this license");
                    }
                    if (!license.Active)
                    {
                        throw new Exception("This license was deactivated");
                    }

                    return new VerifyLicenseResponse(user, license, accessToken);
                }
                catch (ArgumentException ex)
                {
                    throw new ArgumentException(ex.Message);
                }
            }
        }
    }
}
