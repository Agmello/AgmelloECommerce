using Modules.Catalog.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.Catalog.Application
{
    public class CreateCatalogItemHandler
    {
        Domain.Catalog m_catalog { get; }
        ICatalogRepository m_repository { get; }

        public CreateCatalogItemHandler(Catalog.Domain.Catalog catalog, ICatalogRepository catalogRepository)
        {
            m_catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
            m_repository = catalogRepository ?? throw new ArgumentNullException(nameof(catalogRepository));
        }

        public async Task<Guid> HandleAsync(string name, decimal price, string description="", string availability = "Coming soon", string imageUrl = "", IEnumerable<string>? categories = null, IEnumerable<string>? tags = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty.", nameof(name));
            if (price < 0)
                throw new ArgumentOutOfRangeException(nameof(price), "Price cannot be negative.");
            var item = new CatalogItem
            {
                Name = name,
                Description = description,
                Availability = availability,
                Price = price,
                ImageUrl = imageUrl
            };
            if (categories != null)
            {
                item.Categories.AddRange(categories);
            }
            if (tags != null)
            {
                item.Tags.AddRange(tags);
            }
            m_catalog.Items.Add(item);

            await m_repository.AddItemAsyc(item);
            return item.Id;
        }
    }
}
