using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Catalog.Application.Commands;
using Modules.Catalog.Domain;
using Modules.Catalog.Infrastructure;
using Modules.Orders.Domain;
using Modules.Orders.Infrastructure;
using Modules.Users.Application.JWT;
using Modules.Users.Domain;
using Modules.Users.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authorization;
using Modules.Orders.Application.Authorization;

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
            
            void Register<TDbContext, TIRepository, TRepository>(string name, Action? action = null) 
                where TDbContext : DbContext where TIRepository : class where TRepository : class, TIRepository
            {
                if (skip != null && skip.Any(x => x.ToLower() == name)) return;

                // Register Order module services
                services.AddScoped<TIRepository, TRepository>();
                // Add other Order services as needed
                services.AddDbContext<TDbContext>(options => {
                    options.UseSqlServer(configuration.GetConnectionString("AgmelloECommerceDb"));
                });

                action?.Invoke();
            }

            Register<CatalogDbContext,ICatalogRepository, CatalogRepository>("Catalog", () =>
            {
                services.AddSingleton<ICatalog, Catalog.Domain.Catalog>();
                services.AddMediatR(cfg =>
                {
                    cfg.RegisterServicesFromAssembly(typeof(CreateCatalogItemCommandHandler).Assembly);
                });
            });
            Register<UsersDbContext, IUsersRepository, UsersRepository>("user", () =>
            {
                services.AddScoped<IUserJwt, UserJwt>();
            });
            Register<OrdersDbContext, IOrdersRepository, OrdersRepository>("order");
        }

        public static void AddAuthorizations(AuthorizationOptions options)
        {
            options.AddPolicy("CanManageOrder", policy =>
            {
                policy.Requirements.Add(new CanManageOrderRequirment());
            });
        }
    }
}
