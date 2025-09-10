using MediatR;
using Modules.Orders.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.Orders.Application.Queries
{
    internal record GetAllOrderItemsQuery : IRequest<IEnumerable<OrderItems>>;
    internal class GetAllOrderItemsQueryHandler(IOrdersRepository repository) : OrdersHandlerBase<GetAllOrderItemsQuery, IEnumerable<OrderItems>>(repository)
    {
        public override async Task<IEnumerable<OrderItems>> Handle(GetAllOrderItemsQuery request, CancellationToken cancellationToken)
        {

            return await m_repository.GetAllOrderItemsAsync(cancellationToken);
        }
    }
}
