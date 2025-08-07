using Modules.Catalog.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.Catalog.Infrastructure
{
    internal class CatalogRepository : ICatalogRepository
    {
        CatalogDbContext m_dbContext;
        public async Task AddItemAsyc(CatalogItem item, CancellationToken token = default)
        {
            m_dbContext.CatalogItems.Add(item);
            await m_dbContext.SaveChangesAsync(token);
        }

        public async Task DeleteItemAsync(Guid id, CancellationToken token = default)
        {
            var item = m_dbContext.CatalogItems.FirstOrDefault(x => x.Id == id) ?? throw new ArgumentException("Item not found", nameof(id));
            await DeleteItemAsync(item, token);
        }
        public async Task DeleteItemAsync(CatalogItem item, CancellationToken token = default)
        {
            m_dbContext.Remove(item);
            await m_dbContext.SaveChangesAsync(token);
        }

        public async Task<List<CatalogItem>> GetAllAsync(CancellationToken token = default)
        {
            return await m_dbContext.CatalogItems.ToListAsync(token);
        }

        public async Task<CatalogItem> GetByIdAsync(Guid id, CancellationToken token = default)
        {
            return await m_dbContext.CatalogItems
                .FirstOrDefaultAsync(x => x.Id == id, token)
                ?? throw new ArgumentException("Item not found", nameof(id));
        }

        public async Task UpdateFieldAsync(Guid id, string field, object value, CancellationToken token = default)
        {
            var item = m_dbContext.CatalogItems.FirstOrDefault(x => x.Id == id)
                ?? throw new ArgumentException("Item not found", nameof(id));
            item.UpdateField(field, value);
            await m_dbContext.SaveChangesAsync(token);
        }

        public async Task UpdateItemAsync(CatalogItem item, CancellationToken token = default)
        {
            var oldItem = m_dbContext.CatalogItems.FirstOrDefault(x => x.Id == item.Id)
                ?? throw new ArgumentException("Item not found", nameof(item));
            oldItem.Update(item);

            await m_dbContext.SaveChangesAsync(token);
        }
    }
}
