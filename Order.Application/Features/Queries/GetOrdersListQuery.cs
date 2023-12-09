using AutoMapper;
using Entities.Base;
using EstateAgentApi.Services.Base;
using MediatR;
using Microsoft.Extensions.Logging;
using Order.Application.Responses;
using Order.Common.Resources;
using Order.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Order.Application.Features.Queries
{  
    public class GetOrdersListQuery : IRequest<ServiceResult>
    {
        public string Userame { get; set; }
        public GetOrdersListQuery(string username)
        {
            Userame = username;
        }

        public class GetOrdersListQueryHandler : ServiceBase<GetOrdersListQueryHandler>, IRequestHandler<GetOrdersListQuery, ServiceResult>
        {
            private readonly IOrderRepository _order;
            private readonly IMapper _mapper;
            public GetOrdersListQueryHandler(IMapper mapper, ILogger<GetOrdersListQueryHandler> logger, IOrderRepository order) : base(logger)
            {
                _mapper = mapper;
                _order = order;
            }
            public async Task<ServiceResult> Handle(GetOrdersListQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var res = await _order.GetOrdersByUserName(request.Userame);

                    if (res.Data is null)
                        return NotFound(ErrorCodeEnum.NotFound, Resource.NotFound, null);

                    var result = _mapper.Map<List<OrderDto>>(res.Data);

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
