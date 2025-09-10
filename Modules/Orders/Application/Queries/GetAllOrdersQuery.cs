using MediatR;
using Modules.Orders.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.Orders.Application.Queries
{
    internal record GetAllOrdersQuery : IRequest<IEnumerable<Order>>;
    internal class GetAllOrdersQueryHandler(IOrdersRepository repository) : OrdersHandlerBase<GetAllOrdersQuery, IEnumerable<Order>>(repository)
    {
        public override async Task<IEnumerable<Order>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {
            return await m_repository.GetAllAsync(cancellationToken);
        }
    }
}
