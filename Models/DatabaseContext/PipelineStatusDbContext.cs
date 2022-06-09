using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VirtualDean.Models.DatabaseContext
{
    public class PipelineStatusDbContext : DbContext
    {
        public DbSet<Pipelines> PipelineStatus { get; set; }
        public PipelineStatusDbContext(DbContextOptions<PipelineStatusDbContext> options) : base(options)
        {

        }
    }
}
