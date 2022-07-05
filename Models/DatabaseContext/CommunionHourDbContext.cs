using Microsoft.EntityFrameworkCore;

namespace VirtualDean.Models.DatabaseContext
{
    public class CommunionHourDbContext : DbContext
    {
        public DbSet<CommunionOfficeAdded> CommunionHourOffice { get; set; }
        public CommunionHourDbContext(DbContextOptions<CommunionHourDbContext> options) : base(options)
        {

        }
    }
}
