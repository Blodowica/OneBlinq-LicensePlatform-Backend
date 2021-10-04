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

        

        public async Task RegisterLicense(GumroadSaleRequest sale)
        {
            var user = await RegisterBuyer(sale.Email, sale.Purchaser_Id);

            if (sale.Variants == null)
            {
                sale.Variants = new ProductVariants(null);
            }

            var product = await CheckProductInDb(sale.Product_Id, sale.Variants.Tier, sale.Product_Name);
            
            using (var db = contextFactory.CreateDbContext())
            {
                if (product == null)
                {
                    product = await db.Products.FirstOrDefaultAsync(p => p.GumroadID == sale.Product_Id && p.VariantName == sale.Variants.Tier);
                }

                var license = new Licenses
                {
                    PurchaseLocation = sale.Ip_Country,
                    GumroadSubscriptionID = sale.Subscription_Id,
                    GumroadSaleID = sale.Sale_Id,
                    LicenseKey = sale.License_Key,
                    Recurrence = sale.Recurrence,
                    Currency = sale.Currency,
                    Price = sale.Price,
                    Product = product
                };

                user.Licenses.Add(license);
                db.Update(user);
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
                if (await db.Products.FirstOrDefaultAsync(p => p.GumroadID == gumroadID && p.VariantName == variantName) == null)
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
            }
            return null;
        }
    }
}
