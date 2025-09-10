using MediatR;
using Modules.Orders.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.Orders.Application.Commands
{
    internal record PurgeOrderCommand : IRequest<int>;

    internal class PurgeOrderCommandHandler(IOrdersRepository repository) :
        OrdersHandlerBase<PurgeOrderCommand, int>(repository)
    {
        public override async Task<int> Handle(PurgeOrderCommand request, CancellationToken cancellationToken)
        {
            return await m_repository.PurgeAsync(cancellationToken);
        }
    }
}
