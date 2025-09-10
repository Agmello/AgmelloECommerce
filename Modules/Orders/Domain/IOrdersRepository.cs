using Modules.Orders.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.Orders.Domain
{
    internal interface IOrdersRepository
    {
        Task<IEnumerable<Order>> GetAllAsync(CancellationToken cancellationToken);
        Task<IEnumerable<OrderItems>> GetAllOrderItemsAsync(CancellationToken cancellationToken);
        Task<IEnumerable<OrderItems>> GetAllOrderItemsByOrderIdAsync(Guid id, CancellationToken cancellationToken);
        Task<IEnumerable<Order>> GetAllForUserAsync(Guid userId, CancellationToken cancellationToken);
        Task<Order> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<Order> GetByUserAsync(Guid id, CancellationToken cancellationToken);
        Task<Guid> AddAsync(Order order, CancellationToken cancellationToken);
        Task UpdateAsync(Order order, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);

        Task<int> PurgeAsync(CancellationToken token);
    }
}
