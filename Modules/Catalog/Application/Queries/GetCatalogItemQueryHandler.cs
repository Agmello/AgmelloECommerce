using MediatR;
using Modules.Catalog.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.Catalog.Application.Queries
{
    record GetCatalogItemQuery(Guid Id) : IRequest<CatalogItem>;
    internal class GetCatalogItemQueryHandler(ICatalogRepository catalogRepository)
        : CatalogHandlerBase<GetCatalogItemQuery, CatalogItem>(catalogRepository)
    {
        public override async Task<CatalogItem> Handle(GetCatalogItemQuery request, CancellationToken cancellationToken)
        {
            if (request.Id == Guid.Empty)
                throw new ArgumentException("Id cannot be empty", nameof(request.Id));
            return await m_catalogRepository.GetByIdAsync(request.Id, cancellationToken);

        }
    }
}
