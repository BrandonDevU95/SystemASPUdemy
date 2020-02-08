using Microsoft.EntityFrameworkCore;
using SystemASPUdemy.Entities.Warehouse;

namespace SystemASPUdemy.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
    }
}
