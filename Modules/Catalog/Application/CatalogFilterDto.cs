using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.Catalog.Application
{
    public class CatalogFilterDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Availability { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public List<Guid>? CategoryIds { get; set; }
        public List<Guid>? TagIds { get; set; }
    }
}
