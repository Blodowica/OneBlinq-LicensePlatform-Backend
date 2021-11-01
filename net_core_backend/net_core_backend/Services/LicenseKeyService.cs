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
using System.Net.NetworkInformation;
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

        public async Task VerifyLicense(VerifyLicenseRequest model, String token)
        {
            using (var db = contextFactory.CreateDbContext())
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

                var license = user.Licenses.Where(lk => lk.LicenseKey.Equals(model.LicenseKey)).FirstOrDefault();

                if (license == null)
                {
                    throw new ArgumentException("The user does not have this license");
                }
                if (!license.Active)
                {
                    throw new Exception("This license was deactivated");
                }
                else if (license.ExpiresAt > DateTime.UtcNow)
                {
                    license.Active = false;
                    db.Update(license);
                    await db.SaveChangesAsync();

                    throw new Exception("This license has already expired");
                }
            }
        }

        // test code for getting MacAddress
        public String GetMacAddress()
        {
            String firstMacAddress = NetworkInterface
                .GetAllNetworkInterfaces()
                .Where(nic => nic.OperationalStatus == OperationalStatus.Up && nic.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                .Select(nic => nic.GetPhysicalAddress().ToString())
                .FirstOrDefault();

            return firstMacAddress;
        }
    }
}
