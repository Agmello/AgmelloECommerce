using MediatR;
using Modules.Orders.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.Orders.Application.Queries
{
    internal class GetOrderQuery : IRequest<Order>
    {
        public Guid OrderId { get; set; }
    }
    internal class GetOrderQueryHandler(IOrdersRepository repository) : OrdersHandlerBase<GetOrderQuery, Order>(repository)
    {
        public override async Task<Order> Handle(GetOrderQuery request, CancellationToken cancellationToken)
        {
            var order = await m_repository.GetByIdAsync(request.OrderId, cancellationToken);
            order.OrderItems = (await m_repository.GetAllOrderItemsByOrderIdAsync(request.OrderId, cancellationToken)).ToList();
            return order;
        }
    }
}
