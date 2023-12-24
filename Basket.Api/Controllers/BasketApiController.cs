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
using Basket.Core.Entities;
using AutoMapper;
using EventBus.Message.Events;
using Basket.Core.Dtos;
using MassTransit;

namespace Basket.Api.Controllers
{
    [ApiVersion("1")]
    [AllowAnonymous]
    public class BasketApiController : APIControllerBase
    {
        private readonly ILogger<BasketApiController> _logger;
        private ISender _sender;
        private DiscountGrpcService _discountGrpcService;
        private readonly IPublishEndpoint _publishEndpoint;
        private IMapper _mapper;
        public BasketApiController(IPublishEndpoint publishEndpoint,IMapper mapper,DiscountGrpcService discountGrpcService, ILogger<BasketApiController> logger, ISender sender)
        {
            _discountGrpcService = discountGrpcService;
            _logger = logger;
            _sender = sender;
            _publishEndpoint = publishEndpoint;
            _mapper = mapper;
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
                //foreach (var item in model.Items)
                //{
                //    var coupon = await _discountGrpcService.GetDiscount(item.ProductName);

                //    if (coupon.Amount != 0)
                //        item.Price -= coupon.Amount;
                //}
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

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ApiResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResult), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiResult), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Checkout([FromBody] BasketCheckout model)
        {
            try
            {
                //Get existing basket with username
                var data = await _sender.Send(new GetBasketByUsernameQuery(model.UserName));

                if (data.Data is null)
                    return BadRequest();

                var basket = _mapper.Map<ShoppingCartDto>(data.Data);
                
                var eventMessage = _mapper.Map<BasketChekoutEvent>(model);
                eventMessage.TotalPrice = basket.TotalPrice;

               await _publishEndpoint.Publish(eventMessage);

                //Delete the Basket
                await _sender.Send(new DeleteShoppingCartCommand(model.UserName));

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, null, null);
                return InternalServerError(ErrorCodeEnum.InternalError, ex.Message);
            }
        }
    }
}
