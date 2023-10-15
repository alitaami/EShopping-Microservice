using AutoMapper;
using Catalog.Application.Services.Interfaces;
using Catalog.Common.Resources;
using Catalog.Core.Entities.Models;
using Catalog.Core.Entities.Specs;
using Entities.Base;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Features.Properties.Queries
{
    public class GetAllProductsQuery : IRequest<ServiceResult>
    {
        CatalogSearchParams CatalogSearchParams { get; set; }

        public GetAllProductsQuery(CatalogSearchParams catalogSearch)
        {
            CatalogSearchParams = catalogSearch;
        }
        public class GetAllProductsQueryHandler : ServiceBase<GetAllProductsQueryHandler>, IRequestHandler<GetAllProductsQuery, ServiceResult>
        {
            private readonly IProductService _product;
            private readonly IMapper _mapper;
            public GetAllProductsQueryHandler(IMapper mapper, ILogger<GetAllProductsQueryHandler> logger, IProductService product) : base(logger)
            {
                _mapper = mapper;
                _product = product;
            }
            public async Task<ServiceResult> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var res = await _product.GetProducts(request.CatalogSearchParams);

                    if (res.Data is null)
                        return NotFound(ErrorCodeEnum.NotFound, Resource.NotFound, null);

                    var result = _mapper.Map<Pagination<ProductDto>>(res.Data);

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
