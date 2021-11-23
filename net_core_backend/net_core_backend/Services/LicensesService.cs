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
    public class LicensesService : DataService<DefaultModel>, ILicenseKeyService
    {
        private readonly IContextFactory contextFactory;
        public LicensesService(IContextFactory _contextFactory, IOptions<AppSettings> appSettings) : base(_contextFactory)
        {
            contextFactory = _contextFactory;
        }

        public async Task<GetLicenseResponse[]> GetAllLicenses()
        {
            using var db = contextFactory.CreateDbContext();

            var licenses = await db.Licenses
                .Include(x => x.Product)
                .Include(x => x.User)
                .Select(x => new
                {
                    x.LicenseKey,
                    x.Id,
                    x.User.Email,
                    x.Product.MaxUses,
                    Activations = x.ActivationLogs.Count(),
                    x.Active
                })
                .ToArrayAsync();

            List<GetLicenseResponse> response = new List<GetLicenseResponse>();
            foreach(var l in licenses)
            {
                response.Add(new GetLicenseResponse()
                {
                    Activations = l.Activations,
                    Email = l.Email,
                    LicenseId = l.Id,
                    LicenseKey = l.LicenseKey,
                    MaxUses = l.MaxUses,
                    Active = l.Active,
                });
            }

            return response.ToArray();
        }

        public async Task<GetLicenseResponse> GetLicenseDetails(int licenseId)
        {
            using var db = contextFactory.CreateDbContext();

            var l = await db.Licenses
                .Include(x => x.Product)
                .Include(x => x.User)
                .Where(x => x.Id == licenseId)
                .Select(x => new
                {
                    ActivationLogs = x.ActivationLogs.Select(x => new 
                    {
                        x.Id,
                        x.Message,
                        x.Successful
                    }).ToList(),
                    x.LicenseKey,
                    x.Id,
                    x.User.Email,
                    x.Product.MaxUses,
                    x.Product.ProductName,
                    x.Recurrence,
                    x.PurchaseLocation,
                    x.EndedReason,
                    x.ExpiresAt,
                    Activations = x.ActivationLogs.Count(),
                    x.Active
                })
                .FirstOrDefaultAsync();

            if (l == null) return null;

            return new GetLicenseResponse()
            {
                Activations = l.Activations,
                Email = l.Email,
                LicenseId = l.Id,
                LicenseKey = l.LicenseKey,
                MaxUses = l.MaxUses,
                Active = l.Active,
                EndedReason = l.EndedReason,
                ExpiresAt = l.ExpiresAt,
                ProductName = l.ProductName,
                PurchaseLocation = l.PurchaseLocation,
                Recurrence = l.Recurrence,
                ActivationLogs = l.ActivationLogs.Select(x => new GetLicenseResponse.ACLogs()
                {
                    Id = x.Id,
                    Message = x.Message,
                    Successful = x.Successful,
                }).ToList(),
            };
        }

        public async Task VerifyLicense(VerifyLicenseRequest model)
        {
            using (var db = contextFactory.CreateDbContext())
            {
                var license = await db.Licenses.Include(x => x.User).Where(x => x.LicenseKey == model.LicenseKey).FirstOrDefaultAsync();

                if (license == null)
                {
                    throw new ArgumentException("Provided license key does not exist");
                }

                if (!license.Active)
                {
                    throw new Exception("This license is not active");
                }

                // chcecking if the license is opened for the same plugin it was bought for
                bool correctLicense = false;
                foreach (ActivateablePlugins ap in license.Product.ActivateablePlugins)
                {
                    if (ap.Plugin == model.PluginName)
                    {
                        correctLicense = true;
                        break;
                    }
                }

                if (!correctLicense)
                {
                    throw new ArgumentException($"This license can not be used to access plugin \"{model.PluginName}\"");
                }
            }
        }        
    }
}
