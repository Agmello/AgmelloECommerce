using MediatR;
using Modules.Catalog.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.Catalog.Application.Queries
{

    record GetCatalogItemWithFilterQuery(CatalogFilterDto filter)
        : IRequest<List<CatalogItem>>;
    internal class GetCatalogItemWithFilterQueryHandler(ICatalogRepository catalogRepository)
        : CatalogHandlerBase<GetCatalogItemWithFilterQuery, List<CatalogItem>>(catalogRepository)
    {
        public override async Task<List<CatalogItem>> Handle(GetCatalogItemWithFilterQuery request, CancellationToken cancellationToken)
        {
            var filter = request.filter ?? throw new ArgumentException("Filter null");
            return await m_catalogRepository.GetFilteredAsync(filter, cancellationToken);
        }
    }
}
