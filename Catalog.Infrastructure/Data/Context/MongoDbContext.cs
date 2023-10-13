using Catalog.Core.Entities;
using Catalog.Core.Entities.Models;
using Catalog.Infrastructure.Data.SeedData;
using Microsoft.Extensions.Configuration;
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
        private readonly IMongoDatabase _database; // Define the field

        public MongoDbContext(IMongoDatabase database)
        {
            _database = database; // Initialize the field

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

        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return _database.GetCollection<T>(collectionName);
        }
    }
}
