using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore;
using Modules.Catalog.Domain;
using Modules.Catalog.Application.Commands;

namespace Modules
{
    public class ModulesDI
    {
        public static void RegisterServices(IServiceCollection services, IConfiguration configuration,  string[] skip = null)
        {
            // Register your services here
            // For example:
            // services.AddScoped<ICatalogRepository, CatalogRepository>();
            // services.AddScoped<ICatalogService, CatalogService>();
            // services.AddDbContext<CatalogDbContext>(options => options.UseSqlServer("YourConnectionString
            
            void Register(string name, Action action)
            {
                if (skip != null && skip.Any(x => x.ToLower() == "catalog")) return;
                action?.Invoke();
            }

            Register("Catalog", () =>
            {
                // Register Catalog module services
                services.AddScoped<ICatalogRepository, Modules.Catalog.Infrastructure.CatalogRepository>();
                services.AddSingleton<ICatalog, Catalog.Domain.Catalog>();
                // Add other Catalog services as needed
                services.AddDbContext<Modules.Catalog.Infrastructure.CatalogDbContext>(options => {
                    options.UseSqlServer(configuration.GetConnectionString("AgmelloECommerceDb"));

                });
                services.AddMediatR(cfg =>
                {
                    cfg.RegisterServicesFromAssembly(typeof(CreateCatalogItemCommandHandler).Assembly);
                });
            });
        }
    }
}
