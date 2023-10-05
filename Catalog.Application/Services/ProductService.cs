using Catalog.Application.Services.Interfaces;
using Catalog.Core.Entities;
using Entities.Base;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Services
{
    public class ProductService : ServiceBase<ProductService>, IProductService
    {
        IRepository<Product> _repo;
        public ProductService(ILogger<ProductService> logger, IRepository<Product> repo) : base(logger)
        {
            _repo = repo;
        }

        public Task<ServiceResult> CreateProduct(Product product)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> DeleteProduct(string id)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> GetProduct(string id)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> GetProductByBrand(string brand)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> GetProductByName(string name)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> GetProducts()
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> UpdateProduct(Product product)
        {
            throw new NotImplementedException();
        }
    }
}
