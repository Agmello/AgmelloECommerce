using MediatR;
using Modules.Orders.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.Orders.Application.Queries
{
    internal class GetOrderForUserQuery : IRequest<Order>
    {
        public GetOrderForUserQuery(Guid id) {
            UserId = id;
        }
        public Guid UserId { get; set; }
    }
    internal class GetOrderForUserQueryHandler(IOrdersRepository repository) : OrdersHandlerBase<GetOrderForUserQuery, Order>(repository)
    {
        public override async Task<Order> Handle(GetOrderForUserQuery request, CancellationToken cancellationToken)
        {
            var order = await m_repository.GetByIdAsync(request.UserId, cancellationToken);
            order.OrderItems = (await m_repository.GetAllOrderItemsByOrderIdAsync(request.UserId, cancellationToken)).ToList();
            return order;
        }
    }
}
