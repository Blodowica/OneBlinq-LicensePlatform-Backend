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
    public class LoggingService : DataService<DefaultModel>, ILoggingService
    {
        private readonly IContextFactory contextFactory;
        private readonly IMailingService mailingService;

        public LoggingService(IContextFactory _contextFactory, IMailingService mailingService) : base(_contextFactory)
        {
            contextFactory = _contextFactory;
            this.mailingService = mailingService;
        }
        public async Task AddActivationLog(string licenseKey, bool successful, String figmaUserId, string message)
        {
            using var db = contextFactory.CreateDbContext();

            var license = await db.Licenses
                .Include(x => x.User)
                .Include(x => x.ActivationLogs)
                .Include(x => x.Product)
                .Where(x => x.LicenseKey == licenseKey)
                .Select(x => new
                {
                    License = x,
                    UniqueFigmaIds = x.ActivationLogs
                                .Select(a => a.FigmaUserId)
                                .Distinct()
                                .ToList(),
                    ProductMaxUses = x.Product.MaxUses,
                })
                .FirstOrDefaultAsync();

            // If the verification was successful
            // And the verified unique figma id isn't registered yet
            // And if the product max uses is more than 0
            // And if the CURRENT unique figma id count is already at max uses
            // Send an email to the admins
            if (successful && 
                !license.UniqueFigmaIds.Contains(figmaUserId) && 
                license.ProductMaxUses > 0 && 
                license.UniqueFigmaIds.Count() == license.ProductMaxUses)
            {
                mailingService.SendLicenseAbuseEmail(licenseKey, license.License.User.Email);
            }

            ActivationLogs activationLog = null;

            if (license == null)
            {
                activationLog = new ActivationLogs(successful)
                {
                    License = null,
                    FigmaUserId = figmaUserId,
                    Message = message,
                };
            }
            else
            {
                activationLog = new ActivationLogs(successful)
                {
                    License = license.License,
                    FigmaUserId = figmaUserId,
                    Message = message,
                };
            }

            await db.AddAsync(activationLog);
            await db.SaveChangesAsync();
        }

        [Obsolete("This thing doesnt work", true)]
        public String GetMacAddress()
        {
            String firstMacAddress = NetworkInterface
                .GetAllNetworkInterfaces()
                .Where(nic => nic.OperationalStatus == OperationalStatus.Up && nic.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                .Select(nic => nic.GetPhysicalAddress().ToString())
                .FirstOrDefault();

            Console.WriteLine("My Mac address: " + firstMacAddress);

            return firstMacAddress;
        }
    }
}
