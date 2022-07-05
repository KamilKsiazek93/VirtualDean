using System.Linq;
using System.Threading.Tasks;
using VirtualDean.Models.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using VirtualDean.Models;

namespace VirtualDean.Data
{
    public class Week : IWeek
    {
        private readonly WeekDbContext _weekDbContext;
        public Week(WeekDbContext weekDbContext)
        {
            _weekDbContext = weekDbContext;
        }
        public async Task<int> GetLastWeek()
        {
            return await _weekDbContext.WeeksNumber.Select(item => item.WeekNumber)
                .OrderByDescending(item => item).FirstOrDefaultAsync();
        }

        public async Task IncrementWeek()
        {
            int weekNumber = await this.GetLastWeek() + 1;
            await _weekDbContext.AddAsync(new WeekModel{ WeekNumber = weekNumber });
            await _weekDbContext.SaveChangesAsync();
        }
    }
}
