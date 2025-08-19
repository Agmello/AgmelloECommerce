using MediatR;
using Modules.Catalog.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.Catalog.Application.Commands
{
    record DeleteCatalogItemCommand(Guid id) : IRequest<bool>;

    internal class DeleteCatalogItemCommandHandler(ICatalogRepository catalogRepository)
        : CatalogHandlerBase<DeleteCatalogItemCommand, bool>(catalogRepository)
    {
        public override async Task<bool> Handle(DeleteCatalogItemCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));
            var itemId = request.id;
            if (itemId == Guid.Empty)
                throw new ArgumentException("Item ID cannot be empty", nameof(itemId));
            return await m_catalogRepository.DeleteItemAsync(itemId, cancellationToken);
        }
    }
}
