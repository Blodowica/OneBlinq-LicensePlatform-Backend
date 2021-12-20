using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using net_core_backend.Context;
using net_core_backend.Helpers;
using net_core_backend.Models;
using net_core_backend.Models.GumroadRequests;
using net_core_backend.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace net_core_backend.Services
{
    public class GumroadService : DataService<DefaultModel>, IGumroadService
    {
        private readonly IDbContextFactory<OneBlinqDBContext> contextFactory;
        private readonly IMailingService mailingService;
        private readonly AppSettings appSettings;
        private readonly HttpClient httpClient;
        public GumroadService(IDbContextFactory<OneBlinqDBContext> _contextFactory, IOptions<AppSettings> appSettings, HttpClient httpClient, IMailingService mailingService) : base(_contextFactory)
        {
            contextFactory = _contextFactory;
            this.mailingService = mailingService;
            this.appSettings = appSettings.Value;
            this.httpClient = httpClient;
        }

        public async Task RegisterLicense(GumroadSaleRequest request)
        {
            //check if the buyer's email is already in our system if no create it
            var user = await RegisterBuyer(request.email, request.purchaser_id);

            if (request.variants == null)
            {
                request.variants = "Product";
            }

            //check if the product connected to the license already exists in our system if no create it
            var product = await CheckProductInDb(request.product_id, request.variants, request.product_name);
            
            using (var db = contextFactory.CreateDbContext())
            {
                var license = new Licenses
                {
                    PurchaseLocation = request.ip_country,
                    GumroadSubscriptionID = request.subscription_id,
                    GumroadSaleID = request.sale_id,
                    LicenseKey = request.license_key,
                    Recurrence = request.recurrence,
                    Currency = request.currency,
                    Price = float.Parse(request.price),
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
                var license = await db.Licenses.FirstOrDefaultAsync(l => l.GumroadSubscriptionID == request.subscription_id);

                if (license == null)
                {
                    throw new ArgumentException("This license isn't registered in our system");
                }

                if (license.Active == false)
                {
                    throw new ArgumentException("This license is already inactive");
                }

                license.ExpiresAt = DateTime.Parse(request.ended_at);
                license.EndedReason = request.ended_reason;

                db.Update(license);
                await db.SaveChangesAsync();
            }
        }

        public async Task ReactivateLicense(GumroadReactivateRequest request)
        {
            using (var db = contextFactory.CreateDbContext())
            {
                var license = await db.Licenses.FirstOrDefaultAsync(l => l.GumroadSubscriptionID == request.subscription_id);

                if (license == null)
                {
                    throw new ArgumentException("This license isn't registered in our system");
                }

                if (license.Active == true)
                {
                    throw new ArgumentException("This license is already active");
                }

                license.ExpiresAt = null;
                license.EndedReason = "License Restarted";
                license.RestartedAt = DateTime.Parse(request.restarted_at);

                db.Update(license);
                await db.SaveChangesAsync();
            }
        }

        public async Task UpdateLicense(GumroadUpdateRequest request)
        {
            var product = await CheckProductInDb(request.product_id, request.new_plan.tier.name, request.product_id);
            using (var db = contextFactory.CreateDbContext())
            {   
                var license = await db.Licenses.FirstOrDefaultAsync(l => l.GumroadSubscriptionID == request.subscription_id);
                license.Recurrence = request.new_plan.recurrence;
                license.Price = request.new_plan.price_cents;
                license.Product = product;

                db.Update(license);
                await db.SaveChangesAsync();
            }
        }

        public async Task CancelLicense(GumroadCancelRequest request)
        {
            using (var db = contextFactory.CreateDbContext())
            {
                var license = await db.Licenses.FirstOrDefaultAsync(l => l.GumroadSubscriptionID == request.subscription_id);
                
                DateTime expiresAt = license.CreatedAt;
                int recurrenceMonths = 1;

                //find out what the recurrence in months is
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
                //go through the months untill you reach the end of the currently payed for month
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

                    // Send a confirmation to user on account creation
                    mailingService.SendAccountCreationEmail(email);
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

                    //add what plugins should be activate through the tags
                    using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, "https://api.gumroad.com/v2/products/" + gumroadID))
                    {
                        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", appSettings.GumroadAccessToken);

                        var response = await httpClient.SendAsync(requestMessage);

                        if (!response.IsSuccessStatusCode)
                        {
                            throw new ArgumentException("Failed to fetch data from Gumroad");
                        }
                        var result = await response.Content.ReadAsAsync<GumroadProductRequest.GumroadSingleProductRequest>();
                        foreach (var activateablePlugin in result.product.tags)
                        {
                            var activateablePluginToAdd = new ActivateablePlugins
                            {
                                Plugin = activateablePlugin
                            };
                            product.ActivateablePlugins.Add(activateablePluginToAdd);
                        }
                    }

                    await db.AddAsync(product);
                    await db.SaveChangesAsync();

                    return product;
                }
                return foundProduct;
            }
        }

        private string GetCancelReason(GumroadCancelRequest request)
        {
            return "Canceled";
        }
    }
}
