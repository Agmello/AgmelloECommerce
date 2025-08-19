using Modules.Catalog.Application;
using Modules.Catalog.Application.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.Catalog.Domain
{
    public interface ICatalogRepository
    {
        Task<List<CatalogItem>> GetAllAsync(CancellationToken token = default);
        Task<List<CatalogItem>> GetFilteredAsync(CatalogFilterDto filter, CancellationToken token = default);
        Task<CatalogItem> GetByIdAsync(Guid id, CancellationToken token = default);
        Task AddItemAsyc(CatalogItem item, CancellationToken token = default);
        Task UpdateItemAsync(CatalogItem item, CancellationToken token = default);
        Task UpdateFieldAsync(Guid id, string field, object value, CancellationToken token = default);
        Task<bool> DeleteItemAsync(Guid id, CancellationToken token = default);
        Task<int> PurgeAsync(CancellationToken token = default);
    }
}
