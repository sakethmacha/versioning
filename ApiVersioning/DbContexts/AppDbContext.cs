using ApiVersioning.Models;
using Microsoft.EntityFrameworkCore;
namespace ApiVersioning.DbContexts
{
    public class AppDbContext : DbContext
    {
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Category> Categories => Set<Category>();

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
    }

}
