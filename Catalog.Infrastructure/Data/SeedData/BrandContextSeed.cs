using Catalog.Core.Entities;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Catalog.Infrastructure.Data.SeedData
{
    public  class BrandContextSeed
    {
        public static void SeedData(IMongoCollection<ProductBrand> brandCollection)
        {
            bool check = brandCollection.Find(x => true).Any();
            string path = Path.Combine(AppContext.BaseDirectory, "Data", "SeedData", "productBrands.json");
            if (!check)
            {
                var brandsData = File.ReadAllText(path);
                var brands = JsonConvert.DeserializeObject<List<ProductBrand>>(brandsData);
                if (brands != null)
                {
                    foreach (var item in brands)
                    {
                        brandCollection.InsertOneAsync(item);
                    }
                }
            }
        }
    }
}
