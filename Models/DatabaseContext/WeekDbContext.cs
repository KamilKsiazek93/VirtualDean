using Microsoft.EntityFrameworkCore;

namespace VirtualDean.Models.DatabaseContext
{
    public class WeekDbContext : DbContext
    {
        public DbSet<WeekModel> WeeksNumber { get; set; }
        public WeekDbContext(DbContextOptions<WeekDbContext> options) : base(options)
        {

        }
    }
}
