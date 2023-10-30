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

namespace Discount.Application.Features.Commands
{ 
    public class DeleteDiscountCommand : IRequest<ServiceResult>
    {
        public string ProductName { get; set; }
        public DeleteDiscountCommand(string productName)
        {
            ProductName = productName;
        }

        public class DeleteDiscountCommandHandler : ServiceBase<DeleteDiscountCommandHandler>, IRequestHandler<DeleteDiscountCommand, ServiceResult>
        {
            private readonly IDiscountRepository _discount;
            private readonly IMapper _mapper;
            public DeleteDiscountCommandHandler(IMapper mapper, ILogger<DeleteDiscountCommandHandler> logger, IDiscountRepository discount) : base(logger)
            {
                _mapper = mapper;
                _discount = discount;
            }
            public async Task<ServiceResult> Handle(DeleteDiscountCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var res = await _discount.DeleteDiscount(request.ProductName);
                   
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
