using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.Catalog.Domain.ManyToMany
{
    [Table("Category")]
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public List<CatalogItem> Items { get; set; } = new();
    }
}
