using Catalog.Core.Entities;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Infrastructure.Data.SeedData
{
    public static class TypeContextSeed
    {
        public static void SeedData(IMongoCollection<ProductType> collection)
        {
            bool check = collection.Find(x => true).Any();
            string path = Path.Combine("Data", "SeedData", "productTypes.json");
            if (!check)
            {
                var ProductsTypesData = File.ReadAllText(path);
                var types = JsonConvert.DeserializeObject<List<ProductType>>(ProductsTypesData);
                if (types != null)
                {
                    foreach (var item in types)
                    {
                        collection.InsertOneAsync(item);
                    }
                }
            }
        }
    }
}
