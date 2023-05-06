using Catalog.Core.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Catalog.Infrastructure.Data
{
    internal class BrandContextSeed
    {
        public static void SeedData(IMongoCollection<ProductBrand> collection)
        {
            bool ckeckBrands = collection.Find(b => true).Any();

            if (!ckeckBrands)
            {
                string path = Path.Combine("Data", "SeedData", "brands.json");

                string rawBrandData = File.ReadAllText(path);

                IEnumerable<ProductBrand> brands = JsonSerializer.Deserialize<IEnumerable<ProductBrand>>(rawBrandData);

                if (brands.Any())
                {
                    foreach(ProductBrand brand in brands)
                    {
                        collection.InsertOneAsync(brand);
                    }
                }
            }
        }
    }
}
