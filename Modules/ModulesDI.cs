using Microsoft.Extensions.DependencyInjection;
using Modules.Catalog.Application;
using Modules.Catalog.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules
{
    internal class ModulesDI
    {
        public static void RegisterServices(IServiceCollection collection, string[] skip = null)
        {
            // Register your services here
            // For example:
            // services.AddScoped<ICatalogRepository, CatalogRepository>();
            // services.AddScoped<ICatalogService, CatalogService>();
            // services.AddDbContext<CatalogDbContext>(options => options.UseSqlServer("YourConnectionString
            
            void Register(string name, Action action)
            {
                if (skip == null || skip.Any(x => x.ToLower() == "catalog")) return;
                action?.Invoke();
            }

            Register("Catalog", () =>
            {
                // Register Catalog module services
                collection.AddScoped<ICatalogRepository, Modules.Catalog.Infrastructure.CatalogRepository>();
                collection.AddScoped<CreateCatalogItemHandler>();
                collection.AddSingleton<ICatalog,Modules.Catalog.Domain.Catalog>();
                // Add other Catalog services as needed
            });


        }
    }
}
