using MediatR;
using Modules.Orders.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.Orders.Application.Commands
{
    public record class DeleteOrderCommand(Guid id) : IRequest<bool>;

    internal class DeleteOrderCommandHandler(IOrdersRepository repository) : OrdersHandlerBase<DeleteOrderCommand, bool>(repository)
    {
        public override Task<bool> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            return m_repository.DeleteAsync(request.id, cancellationToken);
        }
    }
}
