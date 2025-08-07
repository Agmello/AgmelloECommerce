using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.Catalog.Infrastructure
{
    public class CatalogDbContext : DbContext
    {
        public DbSet<Domain.CatalogItem> CatalogItems => Set<Domain.CatalogItem>();

        public CatalogDbContext(DbContextOptions<CatalogDbContext> options)
            : base(options)
        {
        }
    }
}
