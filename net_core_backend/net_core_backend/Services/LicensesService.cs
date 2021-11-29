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
                    Activations = x.ActivationLogs
                                .Select(a => a.FigmaUserId)
                                .Distinct()
                                .Count(),
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
        
        public async Task<List<GetUserLicenseResponse>> GetAllUserLicenses(int userId)
        {
            using var db = contextFactory.CreateDbContext();

            var licenses = await db.Licenses.
                Where(l => l.UserId ==  userId)
                .Include(x => x.Product)
                .Include(al => al.ActivationLogs)
               .Select(l => new GetUserLicenseResponse
               {
                   id = l.Id,
                   ProductName = l.Product.ProductName,
                   MaxUses = l.Product.MaxUses,
                   Activation = l.ActivationLogs
                                .Select(a => a.FigmaUserId)
                                .Distinct()
                                .Count(),
                   ExpirationDate = l.ExpiresAt,
                   Reaccurence = l.Recurrence,
                   Tier = l.Product.VariantName
                
               })
                .ToListAsync();
            return licenses;
        }

        public async Task toggleLicenseState(int licenseId)
        {
            using (var db = contextFactory.CreateDbContext())
            {
                var license = await db.Licenses.FirstOrDefaultAsync(l => l.Id == licenseId);
                if (license == null)
                {
                    throw new ArgumentException("No license found with given id");
                }
                if (license.Active)
                {
                    license.ExpiresAt = DateTime.UtcNow.AddSeconds(-1);
                    license.EndedReason = "Canceled by admin";
                    license.RestartedAt = null;

                    //logic here to send disable license to gumroad
                }
                else
                {
                    license.ExpiresAt = null;
                    license.EndedReason = null;
                    license.RestartedAt = DateTime.UtcNow;

                    //logic here to send enable license to gumroad
                }

                db.Update(license);
                await db.SaveChangesAsync();
            }
        }

        public async Task VerifyLicense(VerifyLicenseRequest model)
        {
            using (var db = contextFactory.CreateDbContext())
            {
                var license = await db.Licenses.Include(x => x.User)
                    .Include(l => l.Product)
                    .ThenInclude(p => p.ActivateablePlugins)
                    //.Where(ap => ap.Plugin == model.PluginName)
                    .Where(x => x.LicenseKey == model.LicenseKey)
                    .FirstOrDefaultAsync();

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

                foreach(var ap in license.Product.ActivateablePlugins)
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

