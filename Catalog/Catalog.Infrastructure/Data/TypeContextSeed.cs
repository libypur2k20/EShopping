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
    internal class TypeContextSeed
    {
        public static void SeedData(IMongoCollection<ProductType> collection)
        {
            bool ckeckType = collection.Find(t => true).Any();

            if (!ckeckType)
            {
                string path = Path.Combine("bin", "Debug", "net6.0", "Data", "SeedData", "types.json");

                string rawTypeData = File.ReadAllText("../Catalog.Infrstructure/Data/SeedData/types.json");

                IEnumerable<ProductType> types = JsonSerializer.Deserialize<IEnumerable<ProductType>>(rawTypeData);

                if (types.Any())
                {
                    foreach (ProductType type in types)
                    {
                        collection.InsertOneAsync(type);
                    }
                }

            }
        }
    }
}
