using MediatR;
using Modules.Catalog.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.Catalog.Application
{
    internal abstract class CatalogHandlerBase<TAction, TItem> : IRequestHandler<TAction, TItem> where TAction : IRequest<TItem>
    {
        protected readonly ICatalogRepository m_catalogRepository;
        protected CatalogHandlerBase(ICatalogRepository catalogRepository)
        {
            m_catalogRepository = catalogRepository;
        }
        public abstract Task<TItem> Handle(TAction request, CancellationToken cancellationToken);
    }
}
