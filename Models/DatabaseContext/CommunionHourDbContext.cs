using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VirtualDean.Models.DatabaseContext
{
    public class CommunionHourDbContext : DbContext
    {
        public DbSet<CommunionOfficeAdded> CommunionHourOffice { get; set; }
        public CommunionHourDbContext(DbContextOptions<CommunionHourDbContext> options) : base(options)
        {

        }
    }
}
