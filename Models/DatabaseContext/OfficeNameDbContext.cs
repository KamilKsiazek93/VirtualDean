using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VirtualDean.Models.DatabaseContext
{
    public class OfficeNameDbContext : DbContext
    {
        public DbSet<OfficeNames> OfficeNames { get; set; }
        public OfficeNameDbContext(DbContextOptions<OfficeNameDbContext> options): base(options)
        {

        }
    }
}
