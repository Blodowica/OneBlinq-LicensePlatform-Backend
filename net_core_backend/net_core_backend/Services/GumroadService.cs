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
using System.Threading.Tasks;

namespace net_core_backend.Services
{
    public class GumroadService : DataService<DefaultModel>, IGumroadService
    {
        private readonly IContextFactory contextFactory;
        private readonly AppSettings appSettings;
        public GumroadService(IContextFactory _contextFactory, IOptions<AppSettings> appSettings) : base(_contextFactory)
        {
            contextFactory = _contextFactory;
            this.appSettings = appSettings.Value;
        }

        public async Task RegisterLicense(GumroadSaleRequest request)
        {
            var user = await RegisterBuyer(request.Email, request.Purchaser_Id);

            if (request.Variants == null)
            {
                request.Variants = new ProductVariants(null);
            }

            var product = await CheckProductInDb(request.Product_Id, request.Variants.Tier, request.Product_Name);
            
            using (var db = contextFactory.CreateDbContext())
            {
                var license = new Licenses
                {
                    PurchaseLocation = request.Ip_Country,
                    GumroadSubscriptionID = request.Subscription_Id,
                    GumroadSaleID = request.Sale_Id,
                    LicenseKey = request.License_Key,
                    Recurrence = request.Recurrence,
                    Currency = request.Currency,
                    Price = request.Price,
                    Product = product
                };

                user.Licenses.Add(license);
                db.Update(user);
                await db.SaveChangesAsync();
            }
        }

        public async Task DeactivateLicense(GumroadDeactivateRequest request)
        {
            using (var db = contextFactory.CreateDbContext())
            {
                var license = await db.Licenses.FirstOrDefaultAsync(l => l.GumroadSubscriptionID == request.Subscription_Id);

                if (license == null)
                {
                    throw new ArgumentException("This license isn't registered in our system");
                }

                if (license.Active == false)
                {
                    throw new ArgumentException("This license is already inactive");
                }

                license.Active = false;
                license.ExpiresAt = request.Ended_At;
                license.EndedReason = request.Ended_Reason;

                db.Update(license);
                await db.SaveChangesAsync();
            }
        }

        public async Task ReactivateLicense(GumroadReactivateRequest request)
        {
            using (var db = contextFactory.CreateDbContext())
            {
                var license = await db.Licenses.FirstOrDefaultAsync(l => l.GumroadSubscriptionID == request.Subscription_Id);

                if (license == null)
                {
                    throw new ArgumentException("This license isn't registered in our system");
                }

                if (license.Active == true)
                {
                    throw new ArgumentException("This license is already active");
                }

                license.Active = true;
                license.ExpiresAt = null;
                license.EndedReason = "License Restarted";
                license.RestartedAt = request.Restarted_At;

                db.Update(license);
                await db.SaveChangesAsync();
            }
        }

        public async Task UpdateLicense(GumroadUpdateRequest request)
        {
            var product = await CheckProductInDb(request.Product_Id, request.New_Plan.Tier.Name, request.Product_Name);
            using (var db = contextFactory.CreateDbContext())
            {   
                var license = await db.Licenses.FirstOrDefaultAsync(l => l.GumroadSubscriptionID == request.Subscription_Id);
                license.Recurrence = request.New_Plan.Recurrence;
                license.Price = request.New_Plan.Price_Cents;
                license.Product = product;

                db.Update(license);
                await db.SaveChangesAsync();
            }
        }

        public async Task CancelLicense(GumroadCancelRequest request)
        {
            using (var db = contextFactory.CreateDbContext())
            {
                var license = await db.Licenses.FirstOrDefaultAsync(l => l.GumroadSubscriptionID == request.Subscription_Id);
                
                DateTime expiresAt = license.CreatedAt;
                int recurrenceMonths = 1;

                switch (license.Recurrence)
                {
                    case "yearly":
                        recurrenceMonths = 12;
                        break;

                    case "quarterly":
                        recurrenceMonths = 4;
                        break;

                    default:
                        break;
                }
                while (expiresAt < DateTime.UtcNow)
                {
                    expiresAt = expiresAt.AddMonths(recurrenceMonths);
                }

                license.ExpiresAt = expiresAt;
                license.EndedReason = GetCancelReason(request);

                db.Update(license);
                await db.SaveChangesAsync();
            }
        }

        private async Task<Users> RegisterBuyer(string email, string purchaserId)
        {
            using (var db = contextFactory.CreateDbContext())
            {
                var user = await db.Users.FirstOrDefaultAsync(u => u.Email == email);
                if (email != null && user == null)
                {
                    user = new Users
                    {
                        Email = email,
                        GumroadID = purchaserId
                    };

                    await db.AddAsync(user);
                    await db.SaveChangesAsync();
                }
                return user;
            }
        }

        private async Task<Products> CheckProductInDb(string gumroadID, string variantName, string productName)
        {
            using (var db = contextFactory.CreateDbContext())
            {
                var foundProduct = await db.Products.FirstOrDefaultAsync(p => p.GumroadID == gumroadID && p.VariantName == variantName);
                if (foundProduct == null)
                {
                    var product = new Products
                    {
                        ProductName = productName,
                        VariantName = variantName,
                        GumroadID = gumroadID
                    };

                    await db.AddAsync(product);
                    await db.SaveChangesAsync();

                    return product;
                }
                return foundProduct;
            }
        }

        private string GetCancelReason(GumroadCancelRequest request)
        {
            if (request.cancelled_by_admin)
            {
                return "Cancled by admin";
            }

            if (request.cancelled_by_seller)
            {
                return "Cancled by admin";
            }

            if (request.cancelled_by_buyer)
            {
                return "Canceled by user";
            }

            if (request.cancelled_due_to_payment_failures)
            {
                return "Failed payment";
            }

            return "Canceled";
        }
    }
}
