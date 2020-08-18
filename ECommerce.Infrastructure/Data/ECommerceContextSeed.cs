using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ECommerce.Core.Entities;
using ECommerce.Core.Entities.Orders;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ECommerce.Infrastructure.Data
{
    public class ECommerceContextSeed
    {
        public static async Task SeedAsync(ECommerceContext context, ILoggerFactory loggerFactory)
        {
            try
            {
                var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                if (!context.ProductBrands.Any())
                {

                    var brandsData = File.ReadAllText(path + @"/Data/SeedData/brands.json");

                    var brands = JsonConvert.DeserializeObject<List<ProductBrand>>(brandsData);

                    context.ProductBrands.AddRange(brands);

                    await context.SaveChangesAsync();
                }

                if (!context.ProductTypes.Any())
                {
                    var typesData =
                        File.ReadAllText(path + @"/Data/SeedData/types.json");

                    var types = JsonConvert.DeserializeObject<List<ProductType>>(typesData);

                    foreach (var item in types)
                    {
                        context.ProductTypes.Add(item);
                    }

                    await context.SaveChangesAsync();
                }

                if (!context.Products.Any())
                {
                    var productsData =
                        File.ReadAllText(path + @"/Data/SeedData/products.json");

                    var products = JsonConvert.DeserializeObject<List<Product>>(productsData);

                    foreach (var item in products)
                    {
                        context.Products.Add(item);
                    }

                    await context.SaveChangesAsync();
                }

                if (!context.DeliveryMethods.Any())
                {
                    var deliveryMethodData =
                        File.ReadAllText(path + @"/Data/SeedData/delivery.json");

                    var deliveryMethods = JsonConvert.DeserializeObject<List<DeliveryMethod>>(deliveryMethodData);

                    foreach (var item in deliveryMethods)
                    {
                        context.DeliveryMethods.Add(item);
                    }

                    await context.SaveChangesAsync();
                }
            }
            catch (System.Exception ex)
            {
                var logger = loggerFactory.CreateLogger<ECommerceContextSeed>();
                logger.LogError(ex, "Error occured during the seed data");
            }
        }
    }
}