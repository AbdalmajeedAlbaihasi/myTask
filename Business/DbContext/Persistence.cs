using Microsoft.EntityFrameworkCore;
using ProductsDomain;

namespace Products.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public DbSet<Product> Products => Set<Product>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}