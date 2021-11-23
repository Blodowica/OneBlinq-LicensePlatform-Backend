﻿using Microsoft.EntityFrameworkCore;
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

        public async Task<PaginationLicenseResponse> GetLicenses(PaginationLicenseRequest request)
        {
            var globalSearchString = "";
            if (request.GlobalFilter != null)
            {
                //convert active and inactive to status + * (this is to prevent overlap of having the word active in inactive)
                globalSearchString = request.GlobalFilter.ToLower().Replace("inactive", "statusfalse").Replace("active", "statustrue");
            }

            using var db = contextFactory.CreateDbContext();
            var currentTime = DateTime.UtcNow;
            var licenses = await db.Licenses
                .Include(x => x.Product)
                .Include(x => x.User)
                .Include(x => x.ActivationLogs)
                .OrderBy(x => x.Id)
                //Global filtering
                .Where(x => ((x.ExpiresAt <= currentTime) && Convert.ToString(x.Id + x.LicenseKey + x.User.Email + x.ActivationLogs.Count() + "/" + x.Product.MaxUses + x.Product.ProductName + "statusfalse").ToLower()
                    .Contains(globalSearchString)) ||
                    ((x.ExpiresAt > currentTime || x.ExpiresAt == null) && Convert.ToString(x.Id + x.LicenseKey + x.User.Email + x.ActivationLogs.Count() + "/" + x.Product.MaxUses + x.Product.ProductName + "statustrue").ToLower()
                    .Contains(globalSearchString))
                    || globalSearchString == "")
                //Column filtering
                .Where(x => x.Id == request.FilterId || request.FilterId == null)
                .Where(x => x.LicenseKey == request.FilterLicenseKey || request.FilterLicenseKey == null)
                .Where(x => x.User.Email == request.FilterEmail || request.FilterEmail == null)
                .Where(x => x.ActivationLogs.Count() == request.FilterActivation || request.FilterActivation == null)
                .Where(x => x.Product.ProductName == request.FilterProductName || request.FilterProductName == null)
                //Expiration filtering to check if license is active or inactive
                .Where(x => x.ExpiresAt <= currentTime || request.FilterActive == true || request.FilterActive == null)
                .Where(x => x.ExpiresAt > currentTime || x.ExpiresAt == null || request.FilterActive == false || request.FilterActive == null)
                //Pagination
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new PaginationLicenseItem {
                    Activations = x.ActivationLogs.Count(),
                    Active = x.Active,
                    Email = x.User.Email,
                    Id = x.Id,
                    LicenseKey = x.LicenseKey,
                    MaxUses = x.Product.MaxUses,
                    ProductName = x.Product.ProductName
                })
                .ToListAsync();

            PaginationLicenseResponse response = new PaginationLicenseResponse
            {
                MaxPages = 500,
                Licenses = licenses
            };

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
