using AutoMapper;
using Catalog.Application.Features.Properties.Queries;
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

namespace Catalog.Application.Features.Properties.Commands
{
    public class DeleteProductCommand : IRequest<ServiceResult>
    {
        public string Id { get; set; }
        public DeleteProductCommand(string id)
        {
            Id = id;
        }
        public class DeleteProductCommandHandler : ServiceBase<DeleteProductCommandHandler>, IRequestHandler<DeleteProductCommand, ServiceResult>
        {
            private readonly IProductService _product;
            private readonly IMapper _mapper;
            public DeleteProductCommandHandler(IMapper mapper, ILogger<DeleteProductCommandHandler> logger, IProductService product) : base(logger)
            {
                _mapper = mapper;
                _product = product;
            }
            public async Task<ServiceResult> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var res = await _product.DeleteProduct(request.Id); 

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
