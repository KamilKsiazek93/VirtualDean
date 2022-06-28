using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtualDean.Models;
using VirtualDean.Models.DatabaseContext;
using Dapper;
using Microsoft.EntityFrameworkCore;
using VirtualDean.Enties;

namespace VirtualDean.Data
{
    public class TrayCommunionHour : ITrayCommunionHour
    {
        private readonly CommunionHourDbContext _communionContext;
        private readonly TrayHourDbContext _trayContext;
        private readonly HoursDbContext _hourContext;
        private readonly IWeek _week;
        private IEnumerable<string> _communionHours = new Hours().CommunionHours;
        private IEnumerable<string> _trayHours = new Hours().TrayHours;
        public TrayCommunionHour(CommunionHourDbContext communionContext, TrayHourDbContext trayHourDbContext, IWeek week, HoursDbContext hoursDbContext)
        {
            _communionContext = communionContext;
            _trayContext = trayHourDbContext;
            _hourContext = hoursDbContext;
            _week = week;
        }

        public async Task AddCommunionHour(IEnumerable<CommunionOfficeAdded> offices)
        {
            int weekNumber = await _week.GetLastWeek();
            foreach (var office in offices)
            {
                office.WeekOfOffices = weekNumber;
                ValidateHours(office.CommunionHour, _communionHours);
            }

            await _communionContext.AddRangeAsync(offices);
            await _communionContext.SaveChangesAsync();
        }

        public async Task AddTrayHour(IEnumerable<TrayOfficeAdded> offices)
        {
            int weekNumber = await _week.GetLastWeek();
            foreach (var office in offices)
            {
                office.WeekOfOffices = weekNumber;
                ValidateHours(office.TrayHour, _trayHours);
            }

            await _trayContext.AddRangeAsync(offices);
            await _trayContext.SaveChangesAsync();
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
            return await _communionContext.CommunionHourOffice.ToListAsync();
        }

        public async Task<IEnumerable<CommunionOfficeAdded>> GetCommunionHours(int weekId)
        {
            return await _communionContext.CommunionHourOffice.Where(communion => communion.WeekOfOffices == weekId).ToListAsync();
        }
        public async Task<IEnumerable<TrayOfficeAdded>> GetTrayHours()
        {
            return await _trayContext.TrayHourOffice.ToListAsync();
        }

        public async Task<IEnumerable<TrayOfficeAdded>> GetTrayHours(int weekId)
        {
            return await _trayContext.TrayHourOffice.Where(tray => tray.WeekOfOffices == weekId).ToListAsync();
        }

        public async Task<IEnumerable<LastTrayOfficeList>> GetLastTrayHour()
        {
            int weekNumber = await _week.GetLastWeek();
            var ids = await _trayContext.TrayHourOffice.Where(tray => tray.WeekOfOffices == weekNumber)
                .Select(tray => tray.BrotherId).Distinct().ToListAsync();
            var lastTrays = new List<LastTrayOfficeList>();
            
            foreach(var id in ids)
            {
                var trays = await _trayContext.TrayHourOffice.Where(item => item.WeekOfOffices == weekNumber && item.BrotherId == id)
                    .Select(tray => tray.TrayHour).ToListAsync();
                lastTrays.Add(new LastTrayOfficeList { IdBrother = id, BrothersTrays = trays });
            }
            return lastTrays;
        }

        public async Task<IEnumerable<string>> GetTrayHour(int weekNumber, int idBrother)
        {
            return await _trayContext.TrayHourOffice.Where(tray => tray.BrotherId == idBrother && tray.WeekOfOffices == weekNumber)
                .Select(tray => tray.TrayHour).ToListAsync();
        }

        public async Task<IEnumerable<string>> GetCommunionHour(int weekNumber, int idBrother)
        {
            return await _communionContext.CommunionHourOffice.Where(item => item.BrotherId == idBrother && item.WeekOfOffices == weekNumber)
                .Select(item => item.CommunionHour).ToListAsync();
        }

        public async Task<bool> IsTrayAlreadySet()
        {
            int weekNumber = await _week.GetLastWeek();
            return await _trayContext.TrayHourOffice.Where(item => item.WeekOfOffices == weekNumber).AnyAsync();
        }

        public IEnumerable<string> GetHoursForTray()
        {
            return _trayHours;
        }

        public IEnumerable<string> GetHoursForCommunion()
        {
            return _communionHours;
        }
    }
}
