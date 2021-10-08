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

        public async Task<VerifyLicenseResponse> VerifyLicense(VerifyLicenseRequest model, String token)
        {
            using (var db = contextFactory.CreateDbContext())
            {
                try
                {
                    if (await db.AccessTokens.FirstOrDefaultAsync(at => at.AccessToken.Equals(token)) == null)
                    {
                        throw new ArgumentException("Provided access token does not exist");
                    }
                    var user = await db.Users
                        .Include(x => x.Licenses)
                        .Where(x => x.Email == model.Email && x.Licenses
                            .Any(x => x.LicenseKey == model.LicenseKey))
                        .FirstOrDefaultAsync();
                    //if (user == null)
                    //{
                    //    throw new ArgumentException("A user with these credentials does not exist");
                    //}

                    Licenses license = null;

                    foreach (var someLicense in user.Licenses)
                    {
                        if (someLicense.LicenseKey.Equals(model.LicenseKey))
                        {
                            license = someLicense;
                            break;
                        }
                    }

                    if (license == null)
                    {
                        throw new ArgumentException("The user does not have this license");
                    }
                    if (!license.Active)
                    {
                        throw new Exception("This license was deactivated");
                    }
                    else if (license.ExpiresAt.CompareTo(DateTime.Today) < 0)
                    {
                        license.Active = false;
                        db.SaveChanges();

                        throw new Exception("This license has already expired");
                    }

                    return new VerifyLicenseResponse(user, license);
                }
                catch (ArgumentException ex)
                {
                    throw new ArgumentException(ex.Message);
                }
            }
        }
    }
}
