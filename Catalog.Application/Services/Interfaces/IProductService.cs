using Catalog.Core.Entities;
using Catalog.Core.Entities.Models;
using Catalog.Core.Entities.Specs;
using Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Services.Interfaces
{
    public interface IProductService
    {
        public Task<ServiceResult> GetProducts(CatalogSearchParams catalogSearchParams);
        public Task<ServiceResult> GetProduct(string id);
        public Task<ServiceResult> GetProductByName(string name);
        public Task<ServiceResult> GetBrands();
        public Task<ServiceResult> GetTypes();
        public Task<ServiceResult> CreateProduct(Product product);
        public Task<ServiceResult> UpdateProduct(Product product);
        public Task<ServiceResult> DeleteProduct(string id);

    }
}
