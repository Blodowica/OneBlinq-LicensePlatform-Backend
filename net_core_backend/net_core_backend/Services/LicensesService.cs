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
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace net_core_backend.Services
{
    public class LicensesService : DataService<DefaultModel>, ILicenseKeyService
    {
        private readonly AppSettings appSettings;
        private readonly HttpClient httpClient;
        private readonly IDbContextFactory<OneBlinqDBContext> contextFactory;
        private readonly Random random;
        public LicensesService(IDbContextFactory<OneBlinqDBContext> _contextFactory, IOptions<AppSettings> _appSettings, HttpClient _httpClient) : base(_contextFactory)
        {
            contextFactory = _contextFactory;
            appSettings = _appSettings.Value;
            httpClient = _httpClient;
            random = new Random();
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
                    ActivationLogs = x.ActivationLogs
                    .OrderByDescending(a => a.CreatedAt)
                    .Select(x => new
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
                                .Where(a => a.Successful)
                                .Select(a => a.UniqueUserId)
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
                .Include(u => u.User)
                .Include(al => al.ActivationLogs)
               .ThenInclude(un => un.UniqueUser) 
               .Select(l => new GetUserLicenseResponse
               {
                   id = l.Id,
                   LicenseKey= l.LicenseKey,
                   ProductName = l.Product.ProductName,
                   MaxUses = l.Product.MaxUses,
                   Activation = l.ActivationLogs
                                .Where(a => a.Successful)
                                .Select(a => a.UniqueUserId)
                                .Distinct()
                                .Count(),
                   ExpirationDate = l.ExpiresAt,
                   Reaccurence = l.Recurrence,
                   Tier = l.Product.VariantName,
                  Email = l.User.Email,
                  PurchaseLocation = l.PurchaseLocation,
                  EndedReason= l.EndedReason,

                  
                  UniqUsers = l.ActivationLogs.OrderByDescending(x => x.CreatedAt).Where(x => x.Successful).Select(un => new GetUserLicenseResponse.UniqUser { 
                    Id = un.UniqueUser.Id,
                    externalUserId = un.UniqueUser.ExternalUserServiceId,
                    Service= un.UniqueUser.ExternalServiceName,
                    CreatedAt= un.CreatedAt,
                  
                  }).ToList(),
                   
               })
                .ToListAsync();

            foreach (var item in licenses)
            {
             var users =  item.UniqUsers.GroupBy(x => x.Id).Select(x => x.FirstOrDefault()).ToList();
                item.UniqUsers = users;
            }
            return licenses;
        }

        public async Task CreateLicense(string purchaseLocation, string currency, string recurrence, int userId, int price, int productId)
        {
            using (var db = contextFactory.CreateDbContext())
            {
                var user = db.Users.Find(userId) ?? throw new ArgumentException("No user found with given Id");
                var product = db.Products.Find(productId) ?? throw new ArgumentException("No product found with given Id");

                string licenseKey = null;
                while (db.Licenses.FirstOrDefault(l => licenseKey == null || l.LicenseKey == licenseKey) != null)
                {
                    licenseKey = RandomLicenseKey();
                }                

                var license = new Licenses
                {
                    PurchaseLocation = purchaseLocation,
                    Currency = currency,
                    Recurrence = recurrence,
                    UserId = userId,
                    Price = price,
                    ProductId = productId,
                    LicenseKey = licenseKey
                };

                db.Add(license);
                await db.SaveChangesAsync();
            }
        }

        public async Task ToggleLicenseState(int licenseId)
        {
            using (var db = contextFactory.CreateDbContext())
            {
                var license = await db.Licenses.Include(l => l.Product).FirstOrDefaultAsync(l => l.Id == licenseId);
                if (license == null)
                {
                    throw new ArgumentException("No license found with given id");
                }

                string shortURL;
                using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, "https://api.gumroad.com/v2/products/" + license.Product.GumroadID))
                {
                    requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", appSettings.GumroadAccessToken);

                    var response = await httpClient.SendAsync(requestMessage);
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new ArgumentException("Failed to fetch data from Gumroad");
                    }
                    var GumroadProductResult = await response.Content.ReadAsAsync<GumroadProductRequest.GumroadSingleProductRequest>();
                    string URL = GumroadProductResult.product.short_url;
                    shortURL = URL[(URL.LastIndexOf("/") + 1)..];
                }

                if (license.Active)
                {
                    license.ExpiresAt = DateTime.UtcNow.AddSeconds(-1);
                    license.EndedReason = "Canceled by admin";
                    license.RestartedAt = null;

                    await ToggleLicenseGumroadRequest(shortURL, license.LicenseKey, "disable");
                }
                else
                {
                    license.ExpiresAt = null;
                    license.EndedReason = null;
                    license.RestartedAt = DateTime.UtcNow;

                    await ToggleLicenseGumroadRequest(shortURL, license.LicenseKey, "enable");
                }

                db.Update(license);
                await db.SaveChangesAsync();
            }
        }

        private async Task ToggleLicenseGumroadRequest(string shortURL, string licenseKey, string urlSuffix)
        {
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Put, "https://api.gumroad.com/v2/licenses/" + urlSuffix))
            {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", appSettings.GumroadAccessToken);
                //add body as form
                var nvc = new List<KeyValuePair<string, string>>();
                nvc.Add(new KeyValuePair<string, string>("product_permalink", shortURL));
                nvc.Add(new KeyValuePair<string, string>("license_key", licenseKey));

                requestMessage.Content = new FormUrlEncodedContent(nvc);

                var response = await httpClient.SendAsync(requestMessage);
                if (!response.IsSuccessStatusCode)
                {
                    throw new ArgumentException("Failed to fetch data from Gumroad");
                }
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
                    throw new ArgumentException($"This license can not be used to access plugin '{model.PluginName}'");
                }
            }
        }

        private string RandomLicenseKey()
        {
            string randomString = Regex.Replace(Guid.NewGuid().ToString("N").ToUpper(), ".{4}", "$0-");
            return randomString.Remove(19);
        }
    }
}

