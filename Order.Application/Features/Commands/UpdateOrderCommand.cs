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
    public class UpdateOrderCommand : IRequest<ServiceResult>, IValidatable
    {
        public UpdateOrderViewModel model { get; set; }
        public UpdateOrderCommand(UpdateOrderViewModel Model)
        {
            model = Model;
        }

        public class UpdateOrderCommandHandler : ServiceBase<UpdateOrderCommandHandler>, IRequestHandler<UpdateOrderCommand, ServiceResult>
        {
            private readonly IAsyncRepository<Core.Entities.Order> _order;
            private readonly IMapper _mapper;
            public UpdateOrderCommandHandler(IMapper mapper, ILogger<UpdateOrderCommandHandler> logger, IAsyncRepository<Core.Entities.Order> order) : base(logger)
            {
                _mapper = mapper;
                _order = order;
            }
            public async Task<ServiceResult> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
            {
                try
                { 
                    var data = await _order.GetByIdAsync(request.model.Id);
                    
                    if(data is null)
                        return BadRequest(ErrorCodeEnum.BadRequest, Resource.NotFound , null);

                    var res = _mapper.Map<Core.Entities.Order>(request.model);
                   
                    await _order.UpdateAsync(res);

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
}
