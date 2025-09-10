using MediatR;
using Modules.Orders.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.Orders.Application
{
    internal abstract class OrdersHandlerBase<TAction, TItem> : IRequestHandler<TAction, TItem> where TAction : IRequest<TItem>
    {
        protected readonly IOrdersRepository m_repository;
        protected OrdersHandlerBase(IOrdersRepository repository)
        {
            m_repository = repository;
        }
        public abstract Task<TItem> Handle(TAction request, CancellationToken cancellationToken);
    }

    internal abstract class OrdersHandlerBase<TAction> : IRequestHandler<TAction> where TAction : IRequest
    {
        protected readonly IOrdersRepository m_repository;
        protected OrdersHandlerBase(IOrdersRepository repository)
        {
            m_repository = repository;
        }
        public abstract Task Handle(TAction request, CancellationToken cancellationToken);
    }
}
