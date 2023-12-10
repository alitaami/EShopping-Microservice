using AutoMapper;
using Entities.Base;
using EstateAgentApi.Services.Base;
using MediatR;
using Microsoft.Extensions.Logging;
using Order.Common.Resources;
using Order.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Application.Features.Commands
{
    public class DeleteOrderCommand : IRequest<ServiceResult>
    {
        public int Id { get; set; }
        public DeleteOrderCommand(int ID)
        {
            Id = ID;
        }

        public class DeleteOrderCommandHandler : ServiceBase<DeleteOrderCommandHandler>, IRequestHandler<DeleteOrderCommand, ServiceResult>
        {
            private readonly IAsyncRepository<Order.Core.Entities.Order> _order;
            public DeleteOrderCommandHandler(ILogger<DeleteOrderCommandHandler> logger, IAsyncRepository<Order.Core.Entities.Order> order) : base(logger)
            {
                _order = order;
            }
            public async Task<ServiceResult> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var order = await _order.GetByIdAsync(request.Id);

                    if (order is null)
                        return BadRequest(ErrorCodeEnum.BadRequest, Resource.NotFound, null);

                    await _order.DeleteAsync(order);

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
