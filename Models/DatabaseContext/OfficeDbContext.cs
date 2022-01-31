using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VirtualDean.Models.DatabaseContext
{
    public class OfficeDbContext : DbContext
    {
        public DbSet<Office> Offices { get; set; }
        public OfficeDbContext(DbContextOptions<OfficeDbContext> options) : base(options)
        {

        }
    }
}
