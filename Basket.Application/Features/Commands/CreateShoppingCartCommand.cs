using AutoMapper;
using Basket.Common.Resources;
using Basket.Core.Dtos;
using Basket.Core.Entities;
using Basket.Core.Repositories;
using Entities.Base;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Application.Features.Commands
{
    public class CreateShoppingCartCommand : IRequest<ServiceResult>
    {
        public string UserName { get; set; }
        public List<ShoppingCartItem> Items { get; set; }

        public CreateShoppingCartCommand(string name, List<ShoppingCartItem> shoppingCartItems)
        {
            Items = shoppingCartItems;
            UserName = name;
        }
        public class CreateShoppingCartCommandHandler : ServiceBase<CreateShoppingCartCommandHandler>, IRequestHandler<CreateShoppingCartCommand, ServiceResult>
        {
            private readonly IBasketRepository _basket;
            private readonly IMapper _mapper;
            public CreateShoppingCartCommandHandler(IMapper mapper, ILogger<CreateShoppingCartCommandHandler> logger, IBasketRepository basket) : base(logger)
            {
                _mapper = mapper;
                 _basket= basket;
            }
            public async Task<ServiceResult> Handle(CreateShoppingCartCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    //TODO: Call Discount service and apply coupons

                    var res = await _basket.UpdateBasket(new ShoppingCart
                    {
                        Username = request.UserName,
                        Items = request.Items
                    });

                    if (res.Result.Errors.Count()>0)
                        return BadRequest(ErrorCodeEnum.BadRequest, Resource.GeneralErrorTryAgain, null);

                    var result = _mapper.Map<ShoppingCartDto>(res.Data);

                    return Ok(result);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, null, null);
                    return InternalServerError(ErrorCodeEnum.InternalError, ex.Message, null);
                }
            }
        }
    }
}
