using Microsoft.EntityFrameworkCore;

namespace VirtualDean.Models.DatabaseContext
{
    public class TrayHourDbContext : DbContext
    {
        public DbSet<TrayOfficeAdded> TrayHourOffice { get; set; }
        public TrayHourDbContext(DbContextOptions<TrayHourDbContext> options) : base(options)
        {

        }
    }
}
