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
using System.Text;
using System.Threading.Tasks;

namespace Basket.Application.Features.Queries
{
    public class GetBasketByUsernameQuery : IRequest<ServiceResult>
    {
        public string Userame { get; set; }
        public GetBasketByUsernameQuery(string username)
        {
            Userame = username;
        }

        public class GetBasketByUsernameQueryHandler : ServiceBase<GetBasketByUsernameQueryHandler>, IRequestHandler<GetBasketByUsernameQuery, ServiceResult>
        {
            private readonly IBasketRepository _basket;
            private readonly IMapper _mapper;
            public GetBasketByUsernameQueryHandler(IMapper mapper, ILogger<GetBasketByUsernameQueryHandler> logger, IBasketRepository basket) : base(logger)
            {
                _mapper = mapper;
                _basket = basket;
            }
            public async Task<ServiceResult> Handle(GetBasketByUsernameQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var res = await _basket.GetBasket(request.Userame);

                    if (res.Data is null)
                        return NotFound(ErrorCodeEnum.NotFound, Resource.NotFound, null);

                    var result = _mapper.Map<ShoppingCartDto>(res.Data);

                    return Ok(result);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, null, null);
                    return InternalServerError(ErrorCodeEnum.InternalError, Resource.GeneralErrorTryAgain, null);
                }
            }
        }
    }
}
