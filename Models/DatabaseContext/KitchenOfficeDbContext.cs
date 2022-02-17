using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VirtualDean.Models.DatabaseContext
{
    public class KitchenOfficeDbContext : DbContext
    {
        public DbSet<KitchenOffices> KitchenOffice { get; set; }
        public KitchenOfficeDbContext(DbContextOptions<KitchenOfficeDbContext> options) : base(options)
        {

        }
    }
}
