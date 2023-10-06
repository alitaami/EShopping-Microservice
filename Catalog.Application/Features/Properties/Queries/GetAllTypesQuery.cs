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
    public class GetAllTypesQuery : IRequest<ServiceResult>
    {
        public GetAllTypesQuery()
        {

        }
        public class GetAllTypesQueryHandler : ServiceBase<GetAllTypesQueryHandler>, IRequestHandler<GetAllTypesQuery, ServiceResult>
        {
            private readonly IProductService _product;
            private readonly IMapper _mapper;
            public GetAllTypesQueryHandler(IMapper mapper, ILogger<GetAllTypesQueryHandler> logger, IProductService product) : base(logger)
            {
                _mapper = mapper;
                _product = product;
            }
            public async Task<ServiceResult> Handle(GetAllTypesQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var res = await _product.GetTypes();

                    var result = _mapper.Map<IEnumerable<TypesDto>>(res.Data);

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
