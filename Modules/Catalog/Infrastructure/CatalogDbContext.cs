using Microsoft.EntityFrameworkCore;
using Modules.Catalog.Domain;
using Modules.Catalog.Domain.ManyToMany;

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

            modelBuilder.Entity<CatalogItem>()
                .Property(e => e.Id)
                .HasDefaultValueSql("NEWSEQUENTIALID()");
            modelBuilder.Entity<Category>()
                .Property(e => e.Id)
                .HasDefaultValueSql("NEWSEQUENTIALID()");
            modelBuilder.Entity<Tag>()
                .Property(e => e.Id)
                .HasDefaultValueSql("NEWSEQUENTIALID()");
        }
    }
}
