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
                .Where(x => x.LicenseKey.Contains(request.FilterLicenseKey) || request.FilterLicenseKey == "")
                .Where(x => x.User.Email.Contains(request.FilterEmail) || request.FilterEmail == "")
                .Where(x => x.ActivationLogs.Count() == request.FilterActivation || request.FilterActivation == null)
                .Where(x => x.Product.ProductName.Contains(request.FilterProductName) || request.FilterProductName == "")
                //Expiration filtering to check if license is active or inactive
                .Where(x => x.ExpiresAt <= currentTime || request.FilterActive == "active" || request.FilterActive == "")
                .Where(x => x.ExpiresAt > currentTime || x.ExpiresAt == null || request.FilterActive == "inactive" || request.FilterActive == "")
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
            if (maxPages < 1)
            {
                maxPages = 1;
            }
                
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
           
            var filterQuery = db.Users
                .Include(u => u.Licenses)
                .OrderBy(x => x.Id)
                //Global filtering
                .Where(x => (Convert.ToString(x.Id + x.FirstName + x.LastName + x.Email + x.Licenses.Count() + x.Role).ToLower()
                    .Contains(globalSearchString))
                    || globalSearchString == "")
                //Column filtering
                .Where(x => x.Id == request.FilterId || request.FilterId == null)
                .Where(x => x.FirstName.Contains(request.FilterFirstName) || request.FilterFirstName == "")
                .Where(x => x.Email.Contains(request.FilterEmail) || request.FilterEmail == "")
                .Where(x => x.Licenses.Count() == request.FilterLicenseCount || request.FilterLicenseCount == null)
                .Where(x => x.LastName.Contains(request.FilterLastName) || request.FilterLastName == "")
                .Where(x => x.Role.Contains(request.FilterRole) || request.FilterRole == "")
                .AsQueryable();
            //Pagination
            var users = await filterQuery
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new PaginationUserItem
                {
                    LicenseCount = x.Licenses.Count(),
                    FirstName = x.FirstName,
                    Email = x.Email,
                    Id = x.Id,
                    LastName = x.LastName,
                    Role = x.Role
                })
                .ToListAsync();

            int maxPages = (int)Math.Ceiling(filterQuery.Count() / (double)request.PageSize);
            if (maxPages < 1)
            {
                maxPages = 1;
            }

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

            var filterQuery = db.Products
                .Include(p => p.Licenses)
                .OrderBy(x => x.Id)
                //Global filtering
                .Where(x => (x.Active == false) && (Convert.ToString(x.Id + x.ProductName + x.VariantName + x.Active + x.MaxUses + x.Licenses.Count() + "statusfalse").ToLower()
                    .Contains(globalSearchString)) ||
                    (x.Active == true) && (Convert.ToString(x.Id + x.ProductName + x.VariantName + x.Active + x.MaxUses + x.Licenses.Count() + "statustrue").ToLower()
                    .Contains(globalSearchString))
                    || globalSearchString == "")
                //Column filtering
                .Where(x => x.Id == request.FilterId || request.FilterId == null)
                .Where(x => x.ProductName.Contains(request.FilterProductName) || request.FilterProductName == "")
                .Where(x => x.VariantName.Contains(request.FilterVariantName) || request.FilterVariantName == "")
                .Where(x => x.Licenses.Count() == request.FilterLicenseCount || request.FilterLicenseCount == null)
                .Where(x => x.MaxUses == request.FilterMaxUses|| request.FilterMaxUses == null)
                .Where(x => x.Active == request.FilterActive || request.FilterActive == null)
                .AsQueryable();

            //Pagination
            var products = await filterQuery
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new PaginationProductItem
                {
                    LicenseCount = x.Licenses.Count(),
                    ProductName = x.ProductName,
                    VariantName = x.VariantName,
                    Id = x.Id,
                    Active = x.Active,
                    MaxUses = x.MaxUses
                })
                .OrderBy(p => p.ProductName)
                .ToListAsync();

            //Method of getting more generalProductInformation for perhaps a more userfriendly overview
            //List<PaginationGeneralProductItem> generalProductsResponse = new List<PaginationGeneralProductItem>();
            //foreach (var groupedProduct in products.GroupBy(p => p.ProductName))
            //{
            //    int totalLicenses = 0;
            //    List<int> ids = new List<int>();
            //    foreach (var individualProduct in groupedProduct)
            //    {
            //        totalLicenses += individualProduct.LicenseCount;
            //        ids.Add(individualProduct.Id);
            //    }

            //    generalProductsResponse.Add(new PaginationGeneralProductItem
            //    {
            //        Variants = groupedProduct.Count(),
            //        Ids = ids,
            //        Licenses = totalLicenses,
            //        Product = groupedProduct.FirstOrDefault().ProductName
            //    });
            //}


            int maxPages = (int)Math.Ceiling(filterQuery.Count() / (double)request.PageSize);
            if (maxPages < 1)
            {
                maxPages = 1;
            }

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
                .Where(x => (x.Active == false) && (Convert.ToString(x.Id + x.AccessToken + x.User.Email + x.Active + x.CreatedAt.Day + "-" + x.CreatedAt.Month + "-" + x.CreatedAt.Year + "statusfalse").ToLower()
                    .Contains(globalSearchString)) ||
                    (x.Active == true) && (Convert.ToString(x.Id + x.AccessToken + x.User.Email + x.Active + x.CreatedAt.Day + "-" + x.CreatedAt.Month + "-" + x.CreatedAt.Year + "statustrue").ToLower()
                    .Contains(globalSearchString))
                    || globalSearchString == "")
                //Column filtering
                .Where(x => x.Id == request.FilterId || request.FilterId == null)
                .Where(x => x.AccessToken.Contains(request.FilterAccessToken) || request.FilterAccessToken == "")
                .Where(x => x.User.Email.Contains(request.FilterEmail) || request.FilterEmail == "")
                .Where(x => x.Active == request.FilterActive || request.FilterActive == null)
                .Where(x => request.FilterCreatedAt == null || x.CreatedAt.Date == request.FilterCreatedAt.Value.Date.AddDays(1))
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
            if (maxPages < 1)
            {
                maxPages = 1;
            }

            PaginationAccessTokenResponse response = new PaginationAccessTokenResponse
            {
                MaxPages = maxPages,
                AccessTokens = AccessTokens
            };

            return response;

        }
    }
}
