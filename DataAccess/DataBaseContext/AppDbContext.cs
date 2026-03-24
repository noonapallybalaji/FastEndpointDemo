using Orders.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Orders.Data.DataBaseContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }
    }
}
