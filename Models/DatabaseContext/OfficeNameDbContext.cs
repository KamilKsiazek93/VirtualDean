using Microsoft.EntityFrameworkCore;

namespace VirtualDean.Models.DatabaseContext
{
    public class OfficeNameDbContext : DbContext
    {
        public DbSet<OfficeNames> OfficeNames { get; set; }
        public OfficeNameDbContext(DbContextOptions<OfficeNameDbContext> options): base(options)
        {

        }
    }
}
