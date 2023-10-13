using Catalog.Application.Services.Interfaces;
using Catalog.Core.Entities;
using Catalog.Common.Resources;
using Entities.Base;
using Microsoft.Extensions.Logging;

namespace Catalog.Application.Services
{
    public class ProductService : ServiceBase<ProductService>, IProductService
    {
        private readonly  IRepository<Product> _repo;
        private readonly IRepository<ProductBrand> _repoB;
        private readonly IRepository<ProductType> _repoT;
        public ProductService(ILogger<ProductService> logger,  IRepository<ProductBrand> repoB, IRepository<ProductType> repoT, IRepository<Product> repo) : base(logger)
        {
            _repoB = repoB;
            _repoT = repoT;
            _repo = repo;
        }

        public async Task<ServiceResult> CreateProduct(Product product)
        {
            try
            {
                var res = _repo.AddAsync(product);

                if (res.IsFaulted)
                    return BadRequest(ErrorCodeEnum.BadRequest, Resource.CreateError, null);///

                return Ok();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, null, null);

                return InternalServerError(ErrorCodeEnum.InternalError, Resource.GeneralErrorTryAgain, null);
            }
        }

        public async Task<ServiceResult> DeleteProduct(string id)
        {
            try
            {
                var res = _repo.DeleteAsync(id);

                if (res.IsFaulted)
                    return BadRequest(ErrorCodeEnum.BadRequest, Resource.DeleteError, null);///

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, null, null);

                return InternalServerError(ErrorCodeEnum.InternalError, Resource.GeneralErrorTryAgain, null);
            }
        }
         
        public async Task<ServiceResult> GetProduct(string id)
        {
            try
            {
                var res = await _repo.GetByIdAsync(id);

                if (res is null)
                    return NotFound(ErrorCodeEnum.NotFound, Resource.NotFound, null);///

                return Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, null, null);

                return InternalServerError(ErrorCodeEnum.InternalError, Resource.GeneralErrorTryAgain, null);
            }
        }

        public async Task<ServiceResult> GetProductByName(string name)
        {
            try
            {
                var res = _repo.GetAllAsync().Result.Where(x => x.Name == name).FirstOrDefault();

                if (res is null)
                    return NotFound(ErrorCodeEnum.NotFound, Resource.NotFound, null);///

                return Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, null, null);

                return InternalServerError(ErrorCodeEnum.InternalError, Resource.GeneralErrorTryAgain, null);
            }
        }

        public async Task<ServiceResult> GetProducts()
        {
            try
            {
                var res = await _repo.GetAllAsync();

                if (res is null)
                    return NotFound(ErrorCodeEnum.NotFound, Resource.NotFound, null);///

                return Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, null, null);

                return InternalServerError(ErrorCodeEnum.InternalError, Resource.GeneralErrorTryAgain, null);
            }
        }

        public async Task<ServiceResult> GetTypes()
        {
            try
            {
                var res = await _repoT.GetAllAsync();

                if (res is null)
                    return NotFound(ErrorCodeEnum.NotFound, Resource.NotFound, null);///

                return Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, null, null);

                return InternalServerError(ErrorCodeEnum.InternalError, Resource.GeneralErrorTryAgain, null);
            }
        }

        public async Task<ServiceResult> GetBrands()
        {
            try
            {
                var res = await _repoB.GetAllAsync();

                if (res is null)
                    return NotFound(ErrorCodeEnum.NotFound, Resource.NotFound, null);///

                return Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, null, null);

                return InternalServerError(ErrorCodeEnum.InternalError, Resource.GeneralErrorTryAgain, null);
            }
        }

        public async Task<ServiceResult> UpdateProduct(Product product)
        {
            try
            {
                var res = _repo.UpdateAsync(product.Id, product);

                if (res.IsFaulted)
                    return BadRequest(ErrorCodeEnum.BadRequest, Resource.DeleteError, null);///

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, null, null);

                return InternalServerError(ErrorCodeEnum.InternalError, Resource.GeneralErrorTryAgain, null);
            }
        }
    }
}
