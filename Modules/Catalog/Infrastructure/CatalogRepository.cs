using Microsoft.EntityFrameworkCore;
using Modules.Catalog.Application;
using Modules.Catalog.Application.Queries;
using Modules.Catalog.Domain;

namespace Modules.Catalog.Infrastructure
{
    internal class CatalogRepository : ICatalogRepository
    {
        CatalogDbContext m_dbContext;

        public CatalogRepository(CatalogDbContext dbContext)
        {
            m_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task AddItemAsyc(CatalogItem item, CancellationToken token = default)
        {
            m_dbContext.CatalogItems.Add(item);
            await m_dbContext.SaveChangesAsync(token);
        }

        public async Task<bool> DeleteItemAsync(Guid id, CancellationToken token = default)
        {
            var item = m_dbContext.CatalogItems.FirstOrDefault(x => x.Id == id);
            return item is not null ? await DeleteItemAsync(item, token) : false;
        }
        public async Task<bool> DeleteItemAsync(CatalogItem item, CancellationToken token = default)
        {
            m_dbContext.Remove(item);
            return await m_dbContext.SaveChangesAsync(token) > 0;
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

        public Task<List<CatalogItem>> GetFilteredAsync(CatalogFilterDto filter, CancellationToken token = default)
        {
            IQueryable<CatalogItem> query = m_dbContext.CatalogItems;
            if (filter == null)
                throw new ArgumentNullException(nameof(filter), "Filter cannot be null");
            if (!string.IsNullOrWhiteSpace(filter.Name))
                query = query.Where(x => x.Name.ToLower().Contains(filter.Name.ToLower()));
            if (filter.MinPrice.HasValue)
                query = query.Where(x => x.Price >= filter.MinPrice.Value);
            if (filter.MaxPrice.HasValue)
                query = query.Where(x => x.Price <= filter.MaxPrice.Value);
            if (!string.IsNullOrWhiteSpace(filter.Availability))
                query = query.Where(x => x.Availability.ToLower().Equals(filter.Availability.ToLower()));
            if (filter.CategoryIds != null && filter.CategoryIds.Any())
                query = query.Where(x => x.Categories.Any(c => filter.CategoryIds.Contains(c.Id)));
            if (filter.TagIds != null && filter.TagIds.Any())
                query = query.Where(x => x.Tags.Any(t => filter.TagIds.Contains(t.Id)));

            return query.ToListAsync(token);

        }

        public async Task<int> PurgeAsync(CancellationToken token = default)
        {
            var items = await m_dbContext.CatalogItems.ToListAsync(token);
            m_dbContext.CatalogItems.RemoveRange(items);
            return await m_dbContext.SaveChangesAsync(token);
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
