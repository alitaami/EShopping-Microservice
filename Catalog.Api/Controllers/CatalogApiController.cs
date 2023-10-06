using Catalog.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Controllers
{
    [ApiVersion("1")]
    public class CatalogApiController : APIControllerBase
    {
        private readonly ILogger<CatalogApiController> _logger;
        private IProductService _product;
        public CatalogApiController(ILogger<CatalogApiController> logger, IProductService product)
        {
            _logger = logger;
            _product = product;
        }

       
    }
}
