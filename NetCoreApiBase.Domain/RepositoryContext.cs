using Microsoft.EntityFrameworkCore;
using NetCoreApiBase.Domain.Models;

namespace NetCoreApiBase.Domain
{
    public class RepositoryContext : DbContext
    {
        public RepositoryContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }

        //Packages Installed:
        // Install-Package Microsoft.EntityFrameworkCore.Design
        // Install-Package Microsoft.EntityFrameworkCore.SqlServer

    }
}

