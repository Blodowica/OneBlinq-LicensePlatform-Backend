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

                // checking/giving free trial for a plugin
                // (!) check with PO how long a free trial should be

                if (license == null)
                {
                    var freeTrial = await db.FreeTrials.Where(ft => ft.FigmaUserId == model.FigmaUserId && ft.PluginName == ft.PluginName).FirstOrDefaultAsync();
                    
                    if (freeTrial == null)
                    {
                        var ft = new FreeTrials(1);
                        ft.PluginName = model.PluginName;
                        ft.FigmaUserId = model.FigmaUserId;
                        await db.AddAsync(ft);
                        await db.SaveChangesAsync();
                    }

                    else
                    {
                        if (freeTrial.Active && freeTrial.EndDate.CompareTo(DateTime.Today) < 0)
                        {
                            freeTrial.Active = false;
                            await db.AddAsync(freeTrial);
                            await db.SaveChangesAsync();
                        }

                        if (!freeTrial.Active)
                        {
                            throw new ArgumentException("Free trial has expired");
                        }
                        
                    }
                    // should I need this exception any more, as I am going to have free trials instead?
                    throw new ArgumentException("The provided license key does not exist");
                }
                if (!license.Active)
                {
                    throw new Exception("This license is not active");
                }
            }
        }        
    }
}
