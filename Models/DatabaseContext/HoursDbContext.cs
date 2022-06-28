using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VirtualDean.Models.DatabaseContext
{
    public class HoursDbContext : DbContext
    {
        public DbSet<HourModel> Hours { get; set; }
        public HoursDbContext(DbContextOptions<HoursDbContext> options) : base(options)
        {

        }
    }
}
