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
    public class PaginationService : DataService<DefaultModel>, IPaginationService
    {
        private readonly IContextFactory contextFactory;
        public PaginationService(IContextFactory _contextFactory) : base(_contextFactory)
        {
            contextFactory = _contextFactory;
        }

        public async Task<PaginationLicenseResponse> GetLicenses(PaginationLicenseRequest pagination)
        {
            using var db = contextFactory.CreateDbContext();
            var currentTime = DateTime.UtcNow;
            var licenses = await db.Licenses
                .Include(x => x.Product)
                .Include(x => x.User)
                .Include(x => x.ActivationLogs)
                .OrderBy(x => x.Id)
                .Where(x => x.Id == pagination.FilterId || pagination.FilterId == null)
                .Where(x => x.LicenseKey == pagination.FilterLicenseKey || pagination.FilterLicenseKey == null)
                .Where(x => x.User.Email == pagination.FilterEmail || pagination.FilterEmail == null)
                .Where(x => x.ActivationLogs.Count() == pagination.FilterActivation || pagination.FilterActivation == null)
                .Where(x => x.Product.ProductName == pagination.FilterProductName || pagination.FilterProductName == null)
                //Active filtering to check if license is active or inactive
                .Where(x => x.ExpiresAt <= currentTime || pagination.FilterActive == true || pagination.FilterActive == null)
                .Where(x => x.ExpiresAt > currentTime || x.ExpiresAt == null || pagination.FilterActive == false || pagination.FilterActive == null)

                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToArrayAsync();

            PaginationLicenseResponse response = new PaginationLicenseResponse
            {
                MaxPages = 500,
                Licenses = new List<PaginationLicenseItem>()
            };

            foreach (var  license in licenses)
            {
                PaginationLicenseItem item = new PaginationLicenseItem
                {
                    Activations = license.ActivationLogs.Count(),
                    Active = license.Active,
                    Email = license.User.Email,
                    Id = license.Id,
                    LicenseKey = license.LicenseKey,
                    MaxUses = license.Product.MaxUses,
                    ProductName = license.Product.ProductName
                   
                };
                response.Licenses.Add(item);
            }

            return response;


            /*                .OrderBy(x => x.Id)
                            .Skip((license.PageNumber - 1) * license.PageSize)
                            .Take(license.PageSize)
                           */

            //trying to put this in the response header
            /*var paginationMetadata = new PaginatioPagedList(licenses.Count(), license.PageNumber, license.PageSize);*/


            /*            List<GetLicenseResponse> response = new List<GetLicenseResponse>();
                        foreach (var l in licenses)
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

                        return response.ToArray();*/
        }

    }
}
