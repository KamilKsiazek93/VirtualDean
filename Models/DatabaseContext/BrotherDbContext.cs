using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace VirtualDean.Models.DatabaseContext
{
    public class BrotherDbContext : DbContext
    {
        public DbSet<Brother> Brothers { get; set; }
        public BrotherDbContext(DbContextOptions<BrotherDbContext> options) : base(options)
        {

        }
    }
}
