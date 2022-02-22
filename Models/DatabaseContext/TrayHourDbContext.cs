using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VirtualDean.Models.DatabaseContext
{
    public class TrayHourDbContext : DbContext
    {
        public DbSet<TrayOfficeAdded> TrayHourOffice { get; set; }
        public TrayHourDbContext(DbContextOptions<TrayHourDbContext> options) : base(options)
        {

        }
    }
}
