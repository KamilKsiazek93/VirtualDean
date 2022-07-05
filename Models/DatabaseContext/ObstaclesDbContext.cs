using Microsoft.EntityFrameworkCore;

namespace VirtualDean.Models.DatabaseContext
{
    public class ObstaclesDbContext : DbContext
    {
        public DbSet<ObstaclesAdded> Obstacles { get; set; }
        public ObstaclesDbContext(DbContextOptions<ObstaclesDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ObstaclesAdded>().ToTable("Obstacles");
        }
    }
}
