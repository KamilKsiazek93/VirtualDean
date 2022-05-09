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

namespace VirtualDean.Data
{
    public class TrayCommunionHour : ITrayCommunionHour
    {
        private readonly string _connectionString;
        private readonly TrayHourDbContext _trayContext;
        private readonly IWeek _week;
        public TrayCommunionHour(IConfiguration configuration, TrayHourDbContext trayHourDbContext, IWeek week)
        {
            _connectionString = configuration["ConnectionStrings:DefaultConnection"];
            _trayContext = trayHourDbContext;
            _week = week;
        }

        public async Task AddCommunionHour(IEnumerable<CommunionOfficeAdded> offices)
        {
            
        }

        public async Task AddTrayHour(IEnumerable<TrayOfficeAdded> offices)
        {
            int weekNumber = await _week.GetLastWeek();
            foreach (var office in offices)
            {
                office.WeekOfOffices = weekNumber;
            }

            await _trayContext.AddRangeAsync(offices);
            await _trayContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<CommunionOfficeAdded>> GetCommunionHours()
        {
            
        }

        public async Task<IEnumerable<CommunionOfficeAdded>> GetCommunionHours(int weekId)
        {
            
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
    }
}
