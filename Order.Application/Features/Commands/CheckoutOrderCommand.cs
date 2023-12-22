using Application.Features.Behavior.Contracts;
using AutoMapper;
using Entities.Base;
using EstateAgentApi.Services.Base;
using MediatR;
using Microsoft.Extensions.Logging;
using Order.Application.Responses;
using Order.Application.ViewModels;
using Order.Common.Resources;
using Order.Core.Entities;
using Order.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.Features.Commands
{ 
    public class CheckoutOrderCommand : IRequest<ServiceResult> , IValidatable
    {
        public CheckoutOrderViewModel model { get; set; }
        public CheckoutOrderCommand(CheckoutOrderViewModel Model)
        {
            model = Model;
        }

        public class CheckoutOrderCommandHandler : ServiceBase<CheckoutOrderCommandHandler>, IRequestHandler<CheckoutOrderCommand, ServiceResult>
        {
            private readonly IAsyncRepository<Core.Entities.Order> _order;
            private readonly IMapper _mapper;
            public CheckoutOrderCommandHandler(IMapper mapper, ILogger<CheckoutOrderCommandHandler> logger, IAsyncRepository<Core.Entities.Order> order) : base(logger)
            {
                _mapper = mapper;
                _order = order;
            }
            public async Task<ServiceResult> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var data = _mapper.Map<Core.Entities.Order>(request.model);

                    var operation = await _order.AddAsync(data);

                    var res = _mapper.Map<ServiceResult>(operation.Id);

                    return Ok(res);
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
