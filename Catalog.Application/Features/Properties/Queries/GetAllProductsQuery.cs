﻿using AutoMapper;
using Catalog.Application.Services.Interfaces;
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

namespace Catalog.Application.Features.Properties.Queries
{
    public class GetAllProductsQuery : IRequest<ServiceResult>
    {
        public GetAllProductsQuery()
        {

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
                    var res = await _product.GetProducts();

                    var result = _mapper.Map<IEnumerable<ProductDto>>(res.Data);

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