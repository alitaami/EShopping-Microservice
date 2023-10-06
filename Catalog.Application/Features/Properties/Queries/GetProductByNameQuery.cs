using AutoMapper;
using Catalog.Application.Services.Interfaces;
using Catalog.Core.Entities.Models;
using Catalog.Common.Resources;
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
    public class GetProductByNameQuery : IRequest<ServiceResult>
    {
        public string Name { get; set; }
        public GetProductByNameQuery(string name)
        { 
            Name = name;
        }
        public class GetProductByNameQueryHandler : ServiceBase<GetProductByNameQueryHandler>, IRequestHandler<GetProductByNameQuery, ServiceResult>
        {
            private readonly IProductService _product;
            private readonly IMapper _mapper;
            public GetProductByNameQueryHandler(IMapper mapper, ILogger<GetProductByNameQueryHandler> logger, IProductService product) : base(logger)
            {
                _mapper = mapper;
                _product = product;
            }
            public async Task<ServiceResult> Handle(GetProductByNameQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var res = await _product.GetProductByName(request.Name);

                    var result = _mapper.Map<ProductDto>(res.Data);

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
