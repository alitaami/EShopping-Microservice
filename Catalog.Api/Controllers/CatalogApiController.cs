using Catalog.Application.Services.Interfaces;
using Entities.Base;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using System.Net;
using Catalog.Application.Features.Properties.Queries;
using Catalog.Common.Resources;
using Catalog.Application.Features.Properties.Commands;
using Catalog.Core.Entities.Models;
using Catalog.Core.Entities.Specs;

namespace Catalog.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiVersion("1")]
    public class CatalogApiController : APIControllerBase
    {
        private readonly ILogger<CatalogApiController> _logger;
        private IProductService _product;
        private ISender _sender;
        public CatalogApiController(ILogger<CatalogApiController> logger, ISender sender, IProductService product)
        {
            _sender = sender;
            _logger = logger;
            _product = product;
        }

        /// <summary>
        ///  Get the product by its Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ApiResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResult), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiResult), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetProductById(string id)
        {
            try
            {
                var res = await _sender.Send(new GetProductByIdQuery(id));

                return APIResponse(res);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, null, null);
                return InternalServerError(ErrorCodeEnum.InternalError, Resource.GeneralErrorTryAgain);
            }
        }
        // Get all brands
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ApiResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResult), (int)HttpStatusCode.InternalServerError)]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllBrands()
        {
            try
            {
                var result = await _sender.Send(new GetAllBrandsQuery());

                return APIResponse(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, null, null);
                return InternalServerError(ErrorCodeEnum.InternalError, Resource.GeneralErrorTryAgain);
            }
        }

        // Get all products
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ApiResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResult), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetAllProducts([FromQuery]  CatalogSearchParams catalogSearch)
        {
            try
            {
                var result = await _sender.Send(new GetAllProductsQuery(catalogSearch));

                return APIResponse(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, null, null);
                return InternalServerError(ErrorCodeEnum.InternalError, Resource.GeneralErrorTryAgain);
            }
        }

        // Get all types
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ApiResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResult), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetAllTypes()
        {
            try
            {
                var result = await _sender.Send(new GetAllTypesQuery());

                return APIResponse(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, null, null);
                return InternalServerError(ErrorCodeEnum.InternalError, Resource.GeneralErrorTryAgain);
            }
        }

        // Get product by name
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ApiResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResult), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiResult), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetProductByName( string name)
        { 
            try
            {
                if(name is null)
                    return InternalServerError(ErrorCodeEnum.NullField,Resource.NullField);

                var result = await _sender.Send(new GetProductByNameQuery(name));

                return APIResponse(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, null, null);
                return InternalServerError(ErrorCodeEnum.InternalError, Resource.GeneralErrorTryAgain);
            }
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ApiResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResult), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiResult), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> CreateProduct([FromBody] ProductCreateViewModel model)
        {
            try
            {
                var result = await _sender.Send(new CreateProductCommand(model));

                return APIResponse(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, null, null);
                return InternalServerError(ErrorCodeEnum.InternalError, Resource.GeneralErrorTryAgain);
            }
        }

        [HttpDelete("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ApiResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResult), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiResult), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            try
            {
                var result = await _sender.Send(new DeleteProductCommand(id));

                return APIResponse(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, null, null);
                return InternalServerError(ErrorCodeEnum.InternalError, Resource.GeneralErrorTryAgain);
            }
        }

        [HttpPut]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ApiResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResult), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiResult), (int)HttpStatusCode.InternalServerError)]

        public async Task<IActionResult> UpdateProduct([FromBody] ProductUpdateViewModel model)
        {
            try
            {
                var result = await _sender.Send(new UpdateProductCommand(model));

                return APIResponse(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, null, null);
                return InternalServerError(ErrorCodeEnum.InternalError, Resource.GeneralErrorTryAgain);
            }
        }

    }
}
