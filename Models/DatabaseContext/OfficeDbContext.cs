using Microsoft.EntityFrameworkCore;

namespace VirtualDean.Models.DatabaseContext
{
    public class OfficeDbContext : DbContext
    {
        public DbSet<Office> Offices { get; set; }
        public OfficeDbContext(DbContextOptions<OfficeDbContext> options) : base(options)
        {

        }
    }
}
