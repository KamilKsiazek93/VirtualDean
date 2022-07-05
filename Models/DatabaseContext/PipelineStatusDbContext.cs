using Microsoft.EntityFrameworkCore;

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
