using Microsoft.EntityFrameworkCore;
using Modules.Catalog.Infrastructure;
using Modules.Users.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.Users.Infrastructure
{
    public class UsersDbContext : DbContext
    {
        public DbSet<User> Users => Set<User>();

        public UsersDbContext(DbContextOptions<UsersDbContext> options)
            : base(options)
        {
        }

    }
}
