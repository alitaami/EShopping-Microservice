using Catalog.Core.Entities;
using Catalog.Infrastructure.Data.SeedData;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Infrastructure.Data.Context
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;
        public MongoDbContext(IMongoDatabase database)
        {
            // Use the injected database instance
            _database = database;

            // Initialize collections
            ProductBrands = _database.GetCollection<ProductBrand>("ProductBrands");
            Products = _database.GetCollection<Product>("Products");
            ProductTypes = _database.GetCollection<ProductType>("ProductTypes");

            // Seed data
            BrandContextSeed.SeedData(ProductBrands);
            ProductContextSeed.SeedData(Products);
            TypeContextSeed.SeedData(ProductTypes);
        }

        public IMongoCollection<ProductBrand> ProductBrands { get; }
        public IMongoCollection<Product> Products { get; }
        public IMongoCollection<ProductType> ProductTypes { get; }
    }
}
