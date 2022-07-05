using Microsoft.EntityFrameworkCore;

namespace VirtualDean.Models.DatabaseContext
{
    public class BrotherDbContext : DbContext
    {
        public DbSet<Brother> Brothers { get; set; }
        public BrotherDbContext(DbContextOptions<BrotherDbContext> options) : base(options)
        {

        }
    }
}
