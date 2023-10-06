using AutoMapper;
using Catalog.Application.Services.Interfaces;
using Catalog.Core.Entities.Models;
using Catalog.Core.Entities;
using Entities.Base;
using MediatR;
using Microsoft.Extensions.Logging;
using Catalog.Common.Resources;

namespace Catalog.Application.Features.Properties.Commands
{
    public class CreateProductCommand : IRequest<ServiceResult>
    {
        public ProductCreateViewModel model { get; set; }
        public CreateProductCommand(ProductCreateViewModel model)
        {
            this.model = model;
        }
        public class CreateProductCommandHandler : ServiceBase<CreateProductCommandHandler>, IRequestHandler<CreateProductCommand, ServiceResult>
        {
            private readonly IProductService _product;
            private readonly IMapper _mapper;
            public CreateProductCommandHandler(IMapper mapper, ILogger<CreateProductCommandHandler> logger, IProductService product) : base(logger)
            {
                _mapper = mapper;
                _product = product;
            }
            public async Task<ServiceResult> Handle(CreateProductCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var product = _mapper.Map<Product>(request.model);

                    if (product == null)
                        return BadRequest(ErrorCodeEnum.BadRequest, Resource.MapingError, null);///

                    var res = await _product.CreateProduct(product);

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
