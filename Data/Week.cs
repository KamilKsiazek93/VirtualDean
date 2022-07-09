using System.Linq;
using System.Threading.Tasks;
using VirtualDean.Models.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using VirtualDean.Models;

namespace VirtualDean.Data
{
    public class Week : IWeek
    {
        private readonly VirtualDeanDbContext _virtualDeanDbContext;
        public Week(VirtualDeanDbContext virtualDeanDbContext)
        {
            _virtualDeanDbContext = virtualDeanDbContext;
        }
        public async Task<int> GetLastWeek()
        {
            return await _virtualDeanDbContext.WeeksNumber.Select(item => item.WeekNumber)
                .OrderByDescending(item => item).FirstOrDefaultAsync();
        }

        public async Task IncrementWeek()
        {
            int weekNumber = await this.GetLastWeek() + 1;
            await _virtualDeanDbContext.AddAsync(new WeekModel{ WeekNumber = weekNumber });
            await _virtualDeanDbContext.SaveChangesAsync();
        }
    }
}
