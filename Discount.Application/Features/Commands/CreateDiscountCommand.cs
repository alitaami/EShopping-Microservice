using AutoMapper;
using Discount.Common.Resources;
using Discount.Core.Entities;
using Discount.Core.ViewModels;
using Discount.Grpc.Protos;
using Entities.Base;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discount.Application.Features.Commands
{ 
    public class CreateDiscountCommand : IRequest<ServiceResult>
    {
        public CouponViewModel model;

        public CreateDiscountCommand(CouponViewModel model)
        {
            this.model = model;
        }
        public class CreateDiscountCommandHandler : ServiceBase<CreateDiscountCommandHandler>, IRequestHandler<CreateDiscountCommand, ServiceResult>
        {
            private readonly IDiscountRepository _discount;
            private readonly IMapper _mapper;
            public CreateDiscountCommandHandler(IMapper mapper, ILogger<CreateDiscountCommandHandler> logger, IDiscountRepository discount) : base(logger)
            {
                _mapper = mapper;
                _discount = discount;
            }
            public async Task<ServiceResult> Handle(CreateDiscountCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var coupon = _mapper.Map<Coupon>(request.model);

                    await _discount.CreateDiscount(coupon);
  
                    var result = _mapper.Map<CouponModel>(coupon);

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
