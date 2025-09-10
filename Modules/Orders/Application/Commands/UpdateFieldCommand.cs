using MediatR;
using Modules.Orders.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.Orders.Application.Commands
{
    internal record UpdateFieldCommand(Guid OrderId, string Field, object? value) : IRequest;

    internal class UpdateOrderCommandHandler(IOrdersRepository repository) :
        OrdersHandlerBase<UpdateFieldCommand>(repository)
    {
        public override async Task Handle(UpdateFieldCommand request, CancellationToken cancellationToken)
        {
            var order = await m_repository.GetByIdAsync(request.OrderId, cancellationToken);
            // Here you would typically call a method to update the order in the repository
            // e.g., await m_repository.UpdateAsync(order, cancellationToken);
            if(request.Field == nameof(order.Status) && request.value is string value) order.Status = value;
        }
    }


}
