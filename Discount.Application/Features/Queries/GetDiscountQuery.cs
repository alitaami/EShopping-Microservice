using AutoMapper;
using Discount.Common.Resources;
using Discount.Grpc.Protos;
using Entities.Base;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discount.Application.Features.Queries
{
  
    public class GetDiscountQuery : IRequest<ServiceResult>
    {
        public string ProductName { get; set; }
        public GetDiscountQuery(string productName)
        {
            ProductName = productName;
        }

        public class GetDiscountQueryHandler : ServiceBase<GetDiscountQueryHandler>, IRequestHandler<GetDiscountQuery, ServiceResult>
        {
            private readonly IDiscountRepository _discount;
            private readonly IMapper _mapper;
            public GetDiscountQueryHandler(IMapper mapper, ILogger<GetDiscountQueryHandler> logger, IDiscountRepository discount) : base(logger)
            {
                _mapper = mapper;
                _discount = discount;
            }
            public async Task<ServiceResult> Handle(GetDiscountQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var res = await _discount.GetDiscount(request.ProductName);

                    if (res.Data is null)
                        return NotFound(ErrorCodeEnum.NotFound, Resource.NotFound, null);

                    var result = _mapper.Map<CouponModel>(res.Data);

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
