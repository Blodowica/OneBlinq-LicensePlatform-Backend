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
        private readonly IDbContextFactory<OneBlinqDBContext> contextFactory;
        private readonly AppSettings appSettings;
        private readonly HttpClient httpClient;
        public ProductService(IDbContextFactory<OneBlinqDBContext> _contextFactory, IOptions<AppSettings> appSettings, HttpClient httpClient) : base(_contextFactory)
        {
            contextFactory = _contextFactory;
            this.appSettings = appSettings.Value;
            this.httpClient = httpClient;
        }

        public async Task Create(string productName, string variantName, int maxUses)
        {
            if (productName == null || productName == "")
            {
                throw new ArgumentException("Please give a valid productName");
            }

            using (var db = contextFactory.CreateDbContext())
            {
                if (db.Products.FirstOrDefault(p => p.ProductName == productName && p.VariantName == variantName) != null)
                {
                    throw new ArgumentException("Product with given productName and variantName already exists");
                }
                
                var product = new Products
                {
                    ProductName = productName,
                    VariantName = variantName,
                    MaxUses = maxUses
                };

                db.Add(product);
                await db.SaveChangesAsync();
            }
        }

        public async Task RefreshProduct()
        {
            //compare the local and gumroad products to get the local products up to date, this will also generate any new products not in the system
            using (var db = contextFactory.CreateDbContext())
            {
                //get all local products
                var localProducts = await db.Products.Include(p => p.ActivateablePlugins).ToListAsync();
 
                //get all gumroad products
                using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, "https://api.gumroad.com/v2/products/"))
                {
                    requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", appSettings.GumroadAccessToken);

                    var response = await httpClient.SendAsync(requestMessage);
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new ArgumentException("Failed to fetch data from Gumroad");
                    }
                    var GumroadProductsResult = await response.Content.ReadAsAsync<GumroadProductRequest>();

                    //set all products no longer registered on gumroad to inactive
                    foreach (var deletedProduct in localProducts.Where(p => !GumroadProductsResult.products.Any(p2 => p2.id == p.GumroadID)).ToList())
                    {
                        deletedProduct.Active = false;
                        db.Update(deletedProduct);
                        localProducts.Remove(deletedProduct);
                    }
                    await db.SaveChangesAsync();

                    foreach (var gumroadProduct in GumroadProductsResult.products)
                    {
                        var Productvariants = gumroadProduct.variants.Select(v => v.options).ToList().First();
                        //set all variants no longer registed on gumroad to inactive
                        foreach (var deletedProduct in localProducts.Where(p => p.GumroadID == gumroadProduct.id).Where(p2 => !Productvariants.Any(v => v.name == p2.VariantName)).ToList())
                        {
                            deletedProduct.Active = false;
                            db.Update(deletedProduct);
                            localProducts.Remove(deletedProduct);
                        }
                        await db.SaveChangesAsync();

                        //go through every variant from products in gumroad
                        foreach (var allProductsVariant in Productvariants)
                        {
                            var variant = localProducts.FirstOrDefault(p => p.VariantName == allProductsVariant.name && p.GumroadID == gumroadProduct.id);
                            if (variant == null)
                            {
                                //generate a new product variant if there is none locally yet
                                variant = new Products
                                {
                                    GumroadID = gumroadProduct.id,
                                    VariantName = allProductsVariant.name,
                                    ProductName = gumroadProduct.name,
                                    Active = !gumroadProduct.deleted
                                };
                            
                                await db.AddAsync(variant);
                                await db.SaveChangesAsync();
                            }
                            else
                            {
                                //update name and possible state of a product that already exists locally
                                variant.ProductName = gumroadProduct.name;
                                variant.Active = !gumroadProduct.deleted;
                            }

                            //read all tags and add them to the product if they are not there yet
                            foreach (var activateablePlugin in gumroadProduct.tags)
                            {
                                if (!variant.ActivateablePlugins.Any(ap => ap.Plugin == activateablePlugin))
                                {
                                    var activateablePluginToAdd = new ActivateablePlugins
                                    {
                                        Plugin = activateablePlugin
                                    };
                                    variant.ActivateablePlugins.Add(activateablePluginToAdd);
                                }
                            }
                            db.Update(variant);
                            await db.SaveChangesAsync();
                        }
                    }
                }
            }
        }

        public async Task ToggleProduct(int productId)
        {
            using (var db = contextFactory.CreateDbContext())
            {
                var product = await db.Products.FirstOrDefaultAsync(a => a.Id == productId);
                if (product == null)
                {
                    throw new ArgumentException("no Product found with given id");
                }
                product.Active = !product.Active;
                db.Update(product);
                await db.SaveChangesAsync();
            }
        }

        public async Task EditMaxUses(int productId, int maxUses)
        {
            using (var db = contextFactory.CreateDbContext())
            {
                var product = await db.Products.FirstOrDefaultAsync(a => a.Id == productId);
                if (product == null)
                {
                    throw new ArgumentException("no Product found with given id");
                }
                product.MaxUses = maxUses;
                db.Update(product);
                await db.SaveChangesAsync();
            }
        }
    }
}
