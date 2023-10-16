using AutoMapper;
using Basket.Common.Resources;
using Basket.Core.Dtos;
using Basket.Core.Repositories;
using Entities.Base;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Application.Features.Commands
{
    public class DeleteShoppingCartCommand : IRequest<ServiceResult>
    {
        public string Userame { get; set; }
        public DeleteShoppingCartCommand(string username)
        {
            Userame = username;
        }

        public class DeleteShoppingCartCommandHandler : ServiceBase<DeleteShoppingCartCommandHandler>, IRequestHandler<DeleteShoppingCartCommand, ServiceResult>
        {
            private readonly IBasketRepository _basket;
            private readonly IMapper _mapper;
            public DeleteShoppingCartCommandHandler(IMapper mapper, ILogger<DeleteShoppingCartCommandHandler> logger, IBasketRepository basket) : base(logger)
            {
                _mapper = mapper;
                _basket = basket;
            }
            public async Task<ServiceResult> Handle(DeleteShoppingCartCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var res = await _basket.GetBasket(request.Userame);

                    if (res.Data is null)
                        return NotFound(ErrorCodeEnum.NotFound, Resource.NotFound, null);

                   var result = await _basket.DeleteBasket(request.Userame);

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
