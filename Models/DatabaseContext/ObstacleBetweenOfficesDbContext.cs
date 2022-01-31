using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
