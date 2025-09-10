using Azure.Core;
using MediatR;
using Modules.Orders.Application.Dtos;
using Modules.Orders.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.Orders.Application.Commands
{
    internal record AddOrderCommand(AddOrderDto Order) : IRequest<Guid>;

    internal class AddOrderCommandHandler(IOrdersRepository repository) :
        OrdersHandlerBase<AddOrderCommand, Guid>(repository)
    {
        public override async Task<Guid> Handle(AddOrderCommand request, CancellationToken cancellationToken)
        {
            var order = new Order(request.Order);
            
            var ret = await m_repository.AddAsync(order, cancellationToken);
            return ret;
        }
    }
}
