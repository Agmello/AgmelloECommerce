using MediatR;
using Modules.Catalog.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.Catalog.API
{
    internal class GetCatalogItemQuery : IRequest<CatalogItem>
    {
        public Guid Id { get; }
        public GetCatalogItemQuery(Guid id)
        {
            Id = id;// id != Guid.Empty ? id : throw new ArgumentException("Id cannot be empty", nameof(id));
        }
    }

    internal class GetCatalogItemQueryHandler : IRequestHandler<GetCatalogItemQuery, CatalogItem>
    {
        private readonly ICatalogRepository m_catalogRepository;
        public GetCatalogItemQueryHandler(ICatalogRepository catalogRepository)
        {
            m_catalogRepository = catalogRepository ?? throw new ArgumentNullException(nameof(catalogRepository));
        }
        public async Task<CatalogItem> Handle(GetCatalogItemQuery request, CancellationToken cancellationToken)
        {
            if (request.Id == Guid.Empty)
                throw new ArgumentException("Id cannot be empty", nameof(request.Id));
            return await m_catalogRepository.GetByIdAsync(request.Id, cancellationToken);
        }
    }

    record GetAllCatalogItemQuery : IRequest<List<CatalogItem>>;
    internal class GetAllCatalogItemQueryHandler : IRequestHandler<GetAllCatalogItemQuery, List<CatalogItem>>
    {
        private readonly ICatalogRepository m_catalogRepository;
        public GetAllCatalogItemQueryHandler(ICatalogRepository catalogRepository)
        {
            m_catalogRepository = catalogRepository ?? throw new ArgumentNullException(nameof(catalogRepository));
        }
        public async Task<List<CatalogItem>> Handle(GetAllCatalogItemQuery request, CancellationToken cancellationToken)
        {
            return await m_catalogRepository.GetAllAsync(cancellationToken);
        }
    }
}
