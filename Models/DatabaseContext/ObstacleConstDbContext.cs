using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
