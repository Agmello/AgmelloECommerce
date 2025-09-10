using Microsoft.EntityFrameworkCore;
using Modules.Orders.Application.Dtos;
using Modules.Orders.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.Orders.Infrastructure
{
    internal class OrdersRepository : IOrdersRepository
    {
        private readonly OrdersDbContext m_dbContext;
        public OrdersRepository(OrdersDbContext dbContext)
        {
            m_dbContext = dbContext;
        }

        public async Task<Guid> AddAsync(Order order, CancellationToken cancellationToken)
        {
            m_dbContext.Add(order);
            await m_dbContext.SaveChangesAsync(cancellationToken);
            return order.Id;
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var item = await m_dbContext.OrderItems.FirstOrDefaultAsync(order => order.Id == id);
            if (item == null) return false;
            m_dbContext.Remove(item);
            return await m_dbContext.SaveChangesAsync(cancellationToken) > 0;
        }


        public async Task<IEnumerable<Order>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await m_dbContext.Orders.ToListAsync();
        }
        public async Task<IEnumerable<Order>> GetAllForUserAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await m_dbContext.Orders.Where(x => x.UserId == userId).ToListAsync();
        }

        public async Task<IEnumerable<OrderItems>> GetAllOrderItemsAsync(CancellationToken cancellationToken)
        {
            return await m_dbContext.OrderItems.ToListAsync();
        }
        public async Task<IEnumerable<OrderItems>> GetAllOrderItemsByOrderIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await m_dbContext.OrderItems.Where(x => x.OrderId == id).ToListAsync();
        }

        public async Task<Order> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await m_dbContext.Orders.FirstOrDefaultAsync(o => o.Id == id, cancellationToken) ?? throw new ArgumentException($"Not order with id:{id}");
        }

        public Task<Order> GetByUserAsync(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<int> PurgeAsync(CancellationToken token)
        {
            var items = await m_dbContext.Orders.ToListAsync(token);
            m_dbContext.Orders.RemoveRange(items);
            return await m_dbContext.SaveChangesAsync(token);
        }

        public async Task UpdateAsync(Order order, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
