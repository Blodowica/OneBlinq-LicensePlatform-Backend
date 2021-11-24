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
            var filterQuery = db.Licenses
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
                .AsQueryable();

            //Pagination
            var licenses = await filterQuery
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

            int maxPages = (int)Math.Ceiling(filterQuery.Count() / (double)request.PageSize);

            PaginationLicenseResponse response = new PaginationLicenseResponse
            {
                MaxPages = maxPages,
                Licenses = licenses
            };

            return response;

        }

        public async Task<PaginationUserResponse> GetUsers(PaginationUserRequest request)
        {
            var globalSearchString = "";
            if (request.GlobalFilter != null)
            {
                //convert active and inactive to status + * (this is to prevent overlap of having the word active in inactive)
                globalSearchString = request.GlobalFilter.ToLower();
            }

            using var db = contextFactory.CreateDbContext();
           
            var filterQuery = db.Licenses
                .Include(x => x.Product)
                .Include(x => x.User)
                .Include(x => x.ActivationLogs)
                .OrderBy(x => x.Id)
                //Global filtering
                .Where(x => (Convert.ToString(x.Id + x.User.FirstName + x.User.LastName + x.User.Email + x.User.Licenses.Count()).ToLower()
                    .Contains(globalSearchString))
                    || globalSearchString == "")
                //Column filtering
                .Where(x => x.Id == request.FilterId || request.FilterId == null)
                .Where(x => x.User.FirstName == request.FilterFirstName || request.FilterFirstName == null)
                .Where(x => x.User.Email == request.FilterEmail || request.FilterEmail == null)
                .Where(x => x.User.Licenses.Count() == request.FilterLicenseCount || request.FilterLicenseCount == null)
                .Where(x => x.User.LastName == request.FilterLastName || request.FilterLastName == null)
                .AsQueryable();

            //Pagination
            var users = await filterQuery
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new PaginationUserItem
                {
                    LicenseCount = x.User.Licenses.Count(),
                    FirstName = x.User.FirstName,
                    Email = x.User.Email,
                    Id = x.Id,
                    LastName = x.User.LastName,

                })
                .ToListAsync();

            int maxPages = (int)Math.Ceiling(filterQuery.Count() / (double)request.PageSize);

            PaginationUserResponse response = new PaginationUserResponse
            {
                MaxPages = maxPages,
                Users = users
            };

            return response;

        }

        public async Task<PaginationProductResponse> GetProducts(PaginationProductRequest request)
        {
            var globalSearchString = "";
            if (request.GlobalFilter != null)
            {
                //convert active and inactive to status + * (this is to prevent overlap of having the word active in inactive)
                globalSearchString = request.GlobalFilter.ToLower().Replace("inactive", "statusfalse").Replace("active", "statustrue");
            }

            using var db = contextFactory.CreateDbContext();

            var filterQuery = db.Licenses
                .Include(x => x.Product)
                .Include(x => x.User)
                .Include(x => x.ActivationLogs)
                .OrderBy(x => x.Id)
                //Global filtering
                .Where(x => (x.Active == false) && (Convert.ToString(x.Id + x.Product.ProductName + x.Product.VariantName + x.Product.Active + x.Product.MaxUses  + x.Product.Licenses.Count() + "/" + "statusfalse").ToLower()
                    .Contains(globalSearchString)) ||
                    (x.Active == true) && (Convert.ToString(x.Id + x.Product.ProductName + x.Product.VariantName + x.Product.Active + x.Product.MaxUses + x.Product.Licenses.Count() + "/" + "statustrue").ToLower()
                    .Contains(globalSearchString))
                    || globalSearchString == "")
                //Column filtering
                .Where(x => x.Id == request.FilterId || request.FilterId == null)
                .Where(x => x.Product.ProductName == request.FilterProductName || request.FilterProductName == null)
                .Where(x => x.Product.ProductName == request.FilterVariantName|| request.FilterVariantName == null)
                .Where(x => x.Product.Licenses.Count() == request.FilterLicenseCount || request.FilterLicenseCount == null)
                .Where(x => x.Product.MaxUses == request.FilterMaxUses|| request.FilterMaxUses == null)
                .Where(x => x.Product.Active == request.FilterActive || request.FilterActive == null)
                .AsQueryable();

            //Pagination
            var products = await filterQuery
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new PaginationProductItem
                {
                    LicenseCount = x.User.Licenses.Count(),
                    ProductName = x.Product.ProductName,
                    VariantName = x.Product.VariantName,
                    Id = x.Id,
                    Active = x.Product.Active,
                    MaxUses = x.Product.MaxUses

                })
                .ToListAsync();

            int maxPages = (int)Math.Ceiling(filterQuery.Count() / (double)request.PageSize);

            PaginationProductResponse response = new PaginationProductResponse
            {
                MaxPages = maxPages,
                Products = products
            };

            return response;

        }

        public async Task<PaginationAccessTokenResponse> GetAccesTokens(PaginationAccessTokenRequest request)
        {
            var globalSearchString = "";
            if (request.GlobalFilter != null)
            {
                //convert active and inactive to status + * (this is to prevent overlap of having the word active in inactive)
                globalSearchString = request.GlobalFilter.ToLower().Replace("inactive", "statusfalse").Replace("active", "statustrue");
            }

            using var db = contextFactory.CreateDbContext();

            var filterQuery = db.AccessTokens
                .Include(x => x.User)
                .OrderBy(x => x.Id)
                //Global filtering
                .Where(x => (x.Active == false) && (Convert.ToString(x.Id + x.AccessToken + x.User.Email + x.Active + x.CreatedAt + "statusfalse").ToLower()
                    .Contains(globalSearchString)) ||
                    (x.Active == true) && (Convert.ToString(x.Id + x.AccessToken + x.User.Email + x.Active + x.CreatedAt + "statustrue").ToLower()
                    .Contains(globalSearchString))
                    || globalSearchString == "")
                //Column filtering
                .Where(x => x.Id == request.FilterId || request.FilterId == null)
                .Where(x => x.AccessToken == request.FilterAccessToken || request.FilterAccessToken == null)
                .Where(x => x.User.Email == request.FilterEmail || request.FilterEmail == null)
                .Where(x => x.Active == request.FilterActive || request.FilterActive == null)
                .Where(x => x.CreatedAt == request.FilterCreatedAt || request.FilterCreatedAt == null)
                .AsQueryable();

            //Pagination
            var AccessTokens = await filterQuery
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new PaginationAccessTokenItem
                {
                    AccessToken = x.AccessToken,
                    Active = x.Active,
                    Email = x.User.Email,
                    Id = x.Id,
                    CreatedAt = x.CreatedAt,

                })
                .ToListAsync();

            int maxPages = (int)Math.Ceiling(filterQuery.Count() / (double)request.PageSize);

            PaginationAccessTokenResponse response = new PaginationAccessTokenResponse
            {
                MaxPages = maxPages,
                AccessTokens = AccessTokens
            };

            return response;

        }
    }
}
