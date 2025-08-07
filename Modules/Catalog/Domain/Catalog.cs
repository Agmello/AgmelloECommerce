using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.Catalog.Domain
{
    public interface ICatalog
    {
        List<CatalogItem> Items { get; }
    }
    public class Catalog : ICatalog
    {
        public List<CatalogItem> Items { get; } = new();
    }
}
