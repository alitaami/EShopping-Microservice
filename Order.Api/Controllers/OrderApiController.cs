using Entities.Base;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using System.Net;

using Order.Application.Features.Queries;
using Order.Common.Resources;
using Order.Application.ViewModels;
using Order.Application.Features.Commands;
using Basket.Application.Features.Commands;

namespace Catalog.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiVersion("1")]
    [AllowAnonymous]
    public class OrderApiController : APIControllerBase
    {
        private readonly ILogger<OrderApiController> _logger;
        private ISender _sender;
        public OrderApiController(ILogger<OrderApiController> logger, ISender sender)
        {
            _sender = sender;
            _logger = logger;
        }

        /// <summary>
        ///  Get the product by its Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{username}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ApiResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResult), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiResult), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetOrderByUsername(string username)
        {
            try
            {
                var res = await _sender.Send(new GetOrdersListQuery(username));

                return APIResponse(res);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, null, null);
                return InternalServerError(ErrorCodeEnum.InternalError, Resource.GeneralErrorTryAgain);
            }
        }
        [HttpPost("id")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ApiResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResult), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiResult), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> DeleteOrder(int id  )
        {
            try
            {
                var res = await _sender.Send(new DeleteOrderCommand(id));

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
        public async Task<IActionResult> CheckoutOrder([FromBody] CheckoutOrderViewModel model)
        {
            try
            {
                var res = await _sender.Send(new CheckoutOrderCommand(model));

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
        public async Task<IActionResult> UpdateOrder([FromBody] UpdateOrderViewModel model)
        {
            try
            {
                var res = await _sender.Send(new UpdateOrderCommand(model));

                return APIResponse(res);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, null, null);
                return InternalServerError(ErrorCodeEnum.InternalError, Resource.GeneralErrorTryAgain);
            }
        }

    }
}
