using Microsoft.EntityFrameworkCore;

namespace VirtualDean.Models.DatabaseContext
{
    public class ObstacleBetweenOfficesDbContext : DbContext
    {
        public DbSet<ObstacleBetweenOffice> ObstacleBetweenOffices { get; set; }
        public ObstacleBetweenOfficesDbContext(DbContextOptions<ObstacleBetweenOfficesDbContext> options) : base(options)
        {

        }
    }
}
