using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtualDean.Models;
using VirtualDean.Models.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace VirtualDean.Data
{
    public class TrayCommunionHour : ITrayCommunionHour
    {
        private readonly VirtualDeanDbContext _virtualDeanDbContext;
        private readonly IWeek _week;
        public TrayCommunionHour(VirtualDeanDbContext virtualDeanDbContext, IWeek week)
        {
            _virtualDeanDbContext = virtualDeanDbContext;
            _week = week;
        }

        public async Task AddCommunionHour(IEnumerable<CommunionOfficeAdded> offices)
        {
            int weekNumber = await _week.GetLastWeek();
            var communionHours = await GetHoursForCommunion();
            foreach (var office in offices)
            {
                office.WeekOfOffices = weekNumber;
                ValidateHours(office.CommunionHour, communionHours);
            }

            await _virtualDeanDbContext.AddRangeAsync(offices);
            await _virtualDeanDbContext.SaveChangesAsync();
        }

        private void ValidateHours(string office, IEnumerable<string> _listOfHours)
        {
            if (!_listOfHours.Contains(office))
            {
                throw new Exception();
            }
        }

        public async Task<IEnumerable<CommunionOfficeAdded>> GetCommunionHours()
        {
            return await _virtualDeanDbContext.CommunionHourOffice.ToListAsync();
        }

        public async Task<IEnumerable<CommunionOfficeAdded>> GetCommunionHours(int weekId)
        {
            return await _virtualDeanDbContext.CommunionHourOffice.Where(communion => communion.WeekOfOffices == weekId).ToListAsync();
        }

        public async Task<IEnumerable<string>> GetCommunionHour(int weekNumber, int idBrother)
        {
            return await _virtualDeanDbContext.CommunionHourOffice.Where(item => item.BrotherId == idBrother && item.WeekOfOffices == weekNumber)
                .Select(item => item.CommunionHour).ToListAsync();
        }

        public async Task<IEnumerable<string>>GetHoursForCommunion()
        {
            return await _virtualDeanDbContext.Hours.Where(item => item.Hour != "10.30").Select(item => item.Hour).ToListAsync();
        }
    }
}
