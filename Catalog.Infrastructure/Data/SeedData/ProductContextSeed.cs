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
    public class ProductContextSeed
    {
        public static void SeedData(IMongoCollection<Product> collection )
        {
            bool check = collection.Find(x => true).Any();
            string path = Path.Combine(AppContext.BaseDirectory, "Data", "SeedData", "products.json");
            if (!check)
            {
                var ProductsData = File.ReadAllText(path);
                var products = JsonConvert.DeserializeObject<List<Product>>(ProductsData);
                if (products != null)
                {
                    foreach (var item in products)
                    {
                        collection.InsertOneAsync(item);
                    }
                }
            }
        }
    }
}
