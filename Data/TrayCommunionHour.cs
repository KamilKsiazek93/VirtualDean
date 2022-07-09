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

        public async Task AddTrayHour(IEnumerable<TrayOfficeAdded> offices)
        {
            int weekNumber = await _week.GetLastWeek();
            var trayHours = await GetHoursForTray();
            foreach (var office in offices)
            {
                office.WeekOfOffices = weekNumber;
                ValidateHours(office.TrayHour, trayHours);
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
        public async Task<IEnumerable<TrayOfficeAdded>> GetTrayHours()
        {
            return await _virtualDeanDbContext.TrayHourOffice.ToListAsync();
        }

        public async Task<IEnumerable<TrayOfficeAdded>> GetTrayHours(int weekId)
        {
            return await _virtualDeanDbContext.TrayHourOffice.Where(tray => tray.WeekOfOffices == weekId).ToListAsync();
        }

        public async Task<IEnumerable<LastTrayOfficeList>> GetLastTrayHour()
        {
            int weekNumber = await _week.GetLastWeek();
            var ids = await _virtualDeanDbContext.TrayHourOffice.Where(tray => tray.WeekOfOffices == weekNumber)
                .Select(tray => tray.BrotherId).Distinct().ToListAsync();
            var lastTrays = new List<LastTrayOfficeList>();
            
            foreach(var id in ids)
            {
                var trays = await _virtualDeanDbContext.TrayHourOffice.Where(item => item.WeekOfOffices == weekNumber && item.BrotherId == id)
                    .Select(tray => tray.TrayHour).ToListAsync();
                lastTrays.Add(new LastTrayOfficeList { IdBrother = id, BrothersTrays = trays });
            }
            return lastTrays;
        }

        public async Task<IEnumerable<string>> GetTrayHour(int weekNumber, int idBrother)
        {
            return await _virtualDeanDbContext.TrayHourOffice.Where(tray => tray.BrotherId == idBrother && tray.WeekOfOffices == weekNumber)
                .Select(tray => tray.TrayHour).ToListAsync();
        }

        public async Task<IEnumerable<string>> GetCommunionHour(int weekNumber, int idBrother)
        {
            return await _virtualDeanDbContext.CommunionHourOffice.Where(item => item.BrotherId == idBrother && item.WeekOfOffices == weekNumber)
                .Select(item => item.CommunionHour).ToListAsync();
        }

        public async Task<bool> IsTrayAlreadySet()
        {
            int weekNumber = await _week.GetLastWeek();
            return await _virtualDeanDbContext.TrayHourOffice.Where(item => item.WeekOfOffices == weekNumber).AnyAsync();
        }

        public async Task<IEnumerable<string>> GetHoursForTray()
        {
            return await _virtualDeanDbContext.Hours.Select(item => item.Hour).ToListAsync();
        }

        public async Task<IEnumerable<string>>GetHoursForCommunion()
        {
            return await _virtualDeanDbContext.Hours.Where(item => item.Hour != "10.30").Select(item => item.Hour).ToListAsync();
        }
    }
}
