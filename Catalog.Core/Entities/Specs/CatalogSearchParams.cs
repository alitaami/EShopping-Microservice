using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Core.Entities.Specs
{
    public class CatalogSearchParams
    {
        private const int MaxPageSize = 70;
        public int PageIndex { get; set; } = 1;
        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
        public string? BrandId { get; set; }
        public string? TypeId { get; set; }
        public string? Sort { get; set; }
        public string? Search { get; set; }
    }
}
