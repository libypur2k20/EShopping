using Catalog.Core.Entities;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Catalog.Infrastructure.Data
{
    internal class CatalogContextSeed
    {
        public static void SeedData(IMongoCollection<Product> collection)
        {
            bool checkProducts = collection.Find(p => true).Any();

            if (!checkProducts)
            {
                string path = Path.Combine("bin", "Debug", "net6.0", "Data", "SeedData", "products.json");
                string rawData = File.ReadAllText("../Catalog.Infrstructure/Data/SeedData/products.json");
                IEnumerable<Product> products = JsonSerializer.Deserialize<IEnumerable<Product>>(rawData);
                
                if (products.Any())
                {
                    foreach (Product product in products)
                    {
                        collection.InsertOneAsync(product);
                    }
                }
            }
        }
    }
}
