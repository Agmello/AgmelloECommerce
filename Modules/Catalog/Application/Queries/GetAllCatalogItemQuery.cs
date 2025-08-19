using MediatR;
using Modules.Catalog.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.Catalog.Application.Queries
{
    record GetAllCatalogItemQuery : IRequest<List<CatalogItem>>;
    internal class GetAllCatalogItemQueryHandler(ICatalogRepository catalogRepository)
        : CatalogHandlerBase<GetAllCatalogItemQuery,List<CatalogItem>>(catalogRepository) //: IRequestHandler<GetAllCatalogItemQuery, List<CatalogItem>>
    {
        public override async Task<List<CatalogItem>> Handle(GetAllCatalogItemQuery request, CancellationToken cancellationToken)
        {
            return await m_catalogRepository.GetAllAsync(cancellationToken);
        }
    }
}
