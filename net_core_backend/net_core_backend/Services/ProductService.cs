using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using net_core_backend.Context;
using net_core_backend.Helpers;
using net_core_backend.Models;
using net_core_backend.Models.GumroadRequests;
using net_core_backend.Services.Interfaces;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace net_core_backend.Services
{
    public class ProductService : DataService<DefaultModel>, IProductService
    {
        private readonly IContextFactory contextFactory;
        private readonly AppSettings appSettings;
        private readonly HttpClient httpClient;
        public ProductService(IContextFactory _contextFactory, IOptions<AppSettings> appSettings, HttpClient httpClient) : base(_contextFactory)
        {
            contextFactory = _contextFactory;
            this.appSettings = appSettings.Value;
            this.httpClient = httpClient;
        }

        public async Task RefreshProduct(int productId)
        {
            using (var db = contextFactory.CreateDbContext())
            {
                var product = await db.Products.FirstOrDefaultAsync(p => p.Id == productId);
                if (product == null)
                {
                    throw new ArgumentException("No product found with given id");
                }
                var products = await db.Products.Include(p => p.ActivateablePlugins).Where(p => p.ProductName == product.ProductName).ToListAsync();

                //add what plugins should be activate through the tags
                using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, "https://api.gumroad.com/v2/products/" + product.GumroadID))
                {
                    requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", appSettings.GumroadAccessToken);

                    var response = await httpClient.SendAsync(requestMessage);
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new ArgumentException("Failed to fetch data from Gumroad");
                    }
                    var result = await response.Content.ReadAsAsync<GumroadProductRequest>();

                    //go through every variant from product in gumroad 
                    foreach (var productVariant in result.product.variants.Select(v => v.options).ToList().First())
                    {
                        var variant = products.FirstOrDefault(p => p.VariantName == productVariant.name);
                        if (variant == null)
                        {
                            //generate a new product variant if there is none yet
                            variant = new Products
                            {
                                GumroadID = result.product.id,
                                VariantName = productVariant.name,
                                ProductName = result.product.name,
                                Active = !result.product.deleted
                            };
                            
                            await db.AddAsync(variant);
                            await db.SaveChangesAsync();
                        }
                        else
                        {
                            variant.ProductName = result.product.name;
                            variant.Active = !result.product.deleted;
                        }

                        //readd all tags to the product
                        variant.ActivateablePlugins.Clear();
                        foreach (var activateablePlugin in result.product.tags)
                        {
                            var activateablePluginToAdd = new ActivateablePlugins
                            {
                                Plugin = activateablePlugin
                            };
                            variant.ActivateablePlugins.Add(activateablePluginToAdd);
                        }
                        db.Update(variant);
                        await db.SaveChangesAsync();
                    }
                }
            }
        }
    }
}
