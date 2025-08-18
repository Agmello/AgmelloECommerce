using Microsoft.EntityFrameworkCore;
using Modules.Catalog.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Modules.Catalog.Domain.CatalogItem;

namespace Modules.Catalog.Infrastructure
{
    public class CatalogDbContext : DbContext
    {
        private string m_connectionstring;
        public DbSet<Domain.CatalogItem> CatalogItems => Set<Domain.CatalogItem>();

        public CatalogDbContext(DbContextOptions<CatalogDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Domain.CatalogItem>().ToTable("catalog");
            modelBuilder.Entity<Category>().ToTable("Category");
            modelBuilder.Entity<Tag>().ToTable("Tag");

            // EF Core 5+ supports many-to-many without explicit join entity
            modelBuilder.Entity<CatalogItem>()
                .HasMany(i => i.Categories)
                .WithMany(c => c.Items)
                .UsingEntity(j => j.ToTable("ItemCategories"));

            modelBuilder.Entity<CatalogItem>()
                .HasMany(i => i.Tags)
                .WithMany(t => t.Items)
                .UsingEntity(j => j.ToTable("ItemTags"));
        }
    }
}
