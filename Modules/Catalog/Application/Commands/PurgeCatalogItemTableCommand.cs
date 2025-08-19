using MediatR;
using Modules.Catalog.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.Catalog.Application.Commands
{
    public record PurgeCatalogItemTableCommand
        : IRequest<int>;

    internal class PurgeCatalogItemTableCommandHandler(ICatalogRepository catalogRepository)
        : CatalogHandlerBase<PurgeCatalogItemTableCommand, int>(catalogRepository)
    {
        public override async Task<int> Handle(PurgeCatalogItemTableCommand request, CancellationToken cancellationToken)
        {
            return await m_catalogRepository.PurgeAsync(cancellationToken);
        }
    }
}
