using Catalog.Api.Controllers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using System.Net;
using Entities.Base;
using Basket.Application.Features.Queries;
using Basket.Common.Resources;
using Basket.Application.Features.Commands;
using Basket.Application.GrpcService;

namespace Basket.Api.Controllers
{
    [ApiVersion("1")]
    [AllowAnonymous]
    public class BasketApiController : APIControllerBase
    {
        private readonly ILogger<BasketApiController> _logger;
        private ISender _sender;
        private DiscountGrpcService _discountGrpcService;
        public BasketApiController(DiscountGrpcService discountGrpcService, ILogger<BasketApiController> logger, ISender sender)
        {
            _discountGrpcService = discountGrpcService;
            _logger = logger;
            _sender = sender;
        }

        [HttpGet]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ApiResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResult), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiResult), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetGetBasketByUsername(string username)
        {
            try
            {
                var res = await _sender.Send(new GetBasketByUsernameQuery(username));

                return APIResponse(res);
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
        public async Task<IActionResult> UpdateBasket([FromBody] CreateShoppingCartCommand model)
        {
            try
            {
                foreach (var item in model.Items)
                {
                    var coupon = await _discountGrpcService.GetDiscount(item.ProductName);

                    if (coupon.Amount != 0)
                        item.Price -= coupon.Amount;
                }
                var result = await _sender.Send(new CreateShoppingCartCommand(model.UserName, model.Items));

                return APIResponse(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, null, null);
                return InternalServerError(ErrorCodeEnum.InternalError, ex.Message);
            }
        }

        [HttpDelete]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ApiResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResult), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiResult), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> DeleteBasket(string username)
        {
            try
            {
                var result = await _sender.Send(new DeleteShoppingCartCommand(username));

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
