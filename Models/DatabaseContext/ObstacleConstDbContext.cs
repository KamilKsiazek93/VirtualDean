using Microsoft.EntityFrameworkCore;

namespace VirtualDean.Models.DatabaseContext
{
    public class ObstacleConstDbContext : DbContext
    {
        public DbSet<ConstObstacleAdded> ObstacleConst { get; set; }
        public ObstacleConstDbContext(DbContextOptions<ObstacleConstDbContext> options) : base(options)
        {

        }
    }
}
