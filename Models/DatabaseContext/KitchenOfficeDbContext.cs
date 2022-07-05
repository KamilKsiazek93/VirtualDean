using Microsoft.EntityFrameworkCore;

namespace VirtualDean.Models.DatabaseContext
{
    public class KitchenOfficeDbContext : DbContext
    {
        public DbSet<KitchenOffices> KitchenOffice { get; set; }
        public KitchenOfficeDbContext(DbContextOptions<KitchenOfficeDbContext> options) : base(options)
        {

        }
    }
}
