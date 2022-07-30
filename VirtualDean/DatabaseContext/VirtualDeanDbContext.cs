using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VirtualDean.Models.DatabaseContext
{
    public class VirtualDeanDbContext : DbContext
    {
        public DbSet<Brother> Brothers { get; set; }
        public DbSet<CommunionOfficeAdded> CommunionHourOffice { get; set; }
        public DbSet<HourModel> Hours { get; set; }
        public DbSet<KitchenOffices> KitchenOffice { get; set; }
        public DbSet<ObstacleBetweenOffice> ObstacleBetweenOffices { get; set; }
        public DbSet<ConstObstacleAdded> ObstacleConst { get; set; }
        public DbSet<ObstaclesAdded> Obstacles { get; set; }
        public DbSet<Office> Offices { get; set; }
        public DbSet<OfficeNames> OfficeNames { get; set; }
        public DbSet<Pipelines> PipelineStatus { get; set; }
        public DbSet<TrayOfficeAdded> TrayHourOffice { get; set; }
        public DbSet<WeekModel> WeeksNumber { get; set; }

        public VirtualDeanDbContext(DbContextOptions<VirtualDeanDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Brother>().ToTable("Brothers");
            modelBuilder.Entity<CommunionOfficeAdded>().ToTable("CommunionHourOffice");
            modelBuilder.Entity<HourModel>().ToTable("Hours");
            modelBuilder.Entity<KitchenOffices>().ToTable("KitchenOffice");
            modelBuilder.Entity<ObstacleBetweenOffice>().ToTable("ObstacleBetweenOffices");
            modelBuilder.Entity<ConstObstacleAdded>().ToTable("ObstacleConst");
            modelBuilder.Entity<ObstaclesAdded>().ToTable("Obstacles");
            modelBuilder.Entity<Office>().ToTable("Offices");
            modelBuilder.Entity<OfficeNames>().ToTable("OfficeNames");
            modelBuilder.Entity<Pipelines>().ToTable("PipelineStatus");
            modelBuilder.Entity<TrayOfficeAdded>().ToTable("TrayHourOffice");
            modelBuilder.Entity<WeekModel>().ToTable("WeeksNumber");
        }
    }
}
