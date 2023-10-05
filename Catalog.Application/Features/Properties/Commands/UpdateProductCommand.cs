using AutoMapper;
using Catalog.Application.Features.Properties.Queries;
using Catalog.Application.Services.Interfaces;
using Catalog.Core.Entities;
using Catalog.Core.Entities.Models;
using Common.Resources;
using Entities.Base;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Features.Properties.Commands
{
    public class UpdateProductCommand : IRequest<ServiceResult>
    {
        public ProductViewModel model { get; set; }
        public UpdateProductCommand(ProductViewModel model)
        {
            this.model = model;
        }
        public class UpdateProductCommandHandler : ServiceBase<UpdateProductCommandHandler>, IRequestHandler<UpdateProductCommand, ServiceResult>
        {
            private readonly IProductService _product;
            private readonly IMapper _mapper;
            public UpdateProductCommandHandler(IMapper mapper, ILogger<UpdateProductCommandHandler> logger, IProductService product) : base(logger)
            {
                _mapper = mapper;
                _product = product;
            }
            public async Task<ServiceResult> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var product = _mapper.Map<Product>(request.model);

                    if (product == null)
                        return BadRequest(ErrorCodeEnum.BadRequest, Resource.MapingError, null);///

                    var res = await _product.UpdateProduct(product);
                     
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
