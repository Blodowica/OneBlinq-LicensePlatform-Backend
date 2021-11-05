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
        public LoggingService(IContextFactory _contextFactory) : base(_contextFactory)
        {
            contextFactory = _contextFactory;
        }
        public async Task AddActivationLog(string licenseKey, bool successful, String figmaUserId, string message)
        {
            using var db = contextFactory.CreateDbContext();

            Licenses license = db.Licenses.Where(l => l.LicenseKey == licenseKey).FirstOrDefault();
            ActivationLogs activationLog = new ActivationLogs(successful)
            {
                License = license,
                FigmaUserId = figmaUserId,
                Message = message,
            };

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
