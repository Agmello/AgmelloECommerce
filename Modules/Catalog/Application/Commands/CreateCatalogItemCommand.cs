using MediatR;
using Modules.Catalog.Domain;
using Modules.Catalog.Domain.ManyToMany;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Modules.Catalog.Domain.CatalogItem;

namespace Modules.Catalog.Application.Commands
{
    internal class CreateCatalogItemCommand : IRequest<Guid>
    {
        public CatalogItem Item { get; }
        public CreateCatalogItemCommand(string name, string description, string availability, decimal price, string imageUrl, List<Guid> categoryIds = null, List<Guid> tagIds = null)
        {
            Item = new CatalogItem
            {
                Name = name ?? throw new ArgumentNullException(nameof(name)),
                Description = description ?? throw new ArgumentNullException(nameof(description)),
                Availability = availability ?? throw new ArgumentNullException(nameof(availability)),
                Price = price,
                ImageUrl = imageUrl,
                Categories = categoryIds?.Select(id => new Category { Id = id }).ToList(),
                Tags = tagIds?.Select(id => new Tag { Id = id }).ToList()
            };
        }
        public CreateCatalogItemCommand(CatalogItem item)
        {
            Item = item ?? throw new ArgumentNullException(nameof(item));
        }
    }
    internal class CreateCatalogItemCommandHandler(ICatalog catalog, ICatalogRepository catalogRepository) :
        CatalogHandlerBase<CreateCatalogItemCommand, Guid>(catalogRepository)//: IRequestHandler<CreateCatalogItemCommand, Guid>
    {
        ICatalog m_catalog { get; } = catalog ?? throw new ArgumentNullException(nameof(catalog));
        public async Task<Guid> HandleAsync(
            string name,
            decimal price,
            string description="",
            string availability = "Coming soon",
            string imageUrl = "",
            IEnumerable<Category>? categories = null,
            IEnumerable<Tag>? tags = null,
            CancellationToken token = default)
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

            await m_catalogRepository.AddItemAsyc(item, token);
            return item.Id;
        }
        public async Task<Guid> HandleAsync(CatalogItem item, CancellationToken token = default)
        {
            m_catalog.Items.Add(item);

            await m_catalogRepository.AddItemAsyc(item, token);
            return item.Id;
        }

        /*protected override async Task<Guid> HandleAsync(CreateCatalogItemCommand request, CancellationToken cancellationToken)
        {
            return await HandleAsync(request.Item, cancellationToken);
        }
        async Task<Guid> IRequestHandler<CreateCatalogItemCommand, Guid>.Handle(CreateCatalogItemCommand request, CancellationToken cancellationToken)
        {
            return await HandleAsync(request.Item, cancellationToken);
        }*/

        public override async Task<Guid> Handle(CreateCatalogItemCommand request, CancellationToken cancellationToken)
        {
            return await HandleAsync(request.Item, cancellationToken);
        }
    }
}
