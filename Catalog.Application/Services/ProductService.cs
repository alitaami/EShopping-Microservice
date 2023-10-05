﻿using Catalog.Application.Services.Interfaces;
using Catalog.Core.Entities;
using Common.Resources;
using Entities.Base;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Catalog.Application.Services
{
    public class ProductService : ServiceBase<ProductService>, IProductService
    {
        IRepository<Product> _repo;
        IRepository<ProductBrand> _repoB;
        IRepository<ProductType> _repoT;
        public ProductService(ILogger<ProductService> logger, IRepository<ProductBrand> repoB, IRepository<ProductType> repoT, IRepository<Product> repo) : base(logger)
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

                if (res.IsCanceled)
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

                if (res.IsCanceled)
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

                if (res.IsCanceled)
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
