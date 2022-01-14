using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
