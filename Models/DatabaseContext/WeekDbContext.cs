using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VirtualDean.Models.DatabaseContext
{
    public class WeekDbContext : DbContext
    {
        public DbSet<WeekModel> WeeksNumber { get; set; }
        public WeekDbContext(DbContextOptions<WeekDbContext> options) : base(options)
        {

        }
    }
}
