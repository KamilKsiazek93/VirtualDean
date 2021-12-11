using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtualDean.Models;
using Dapper;

namespace VirtualDean.Data
{
    public class TrayCommunionHour : ITrayCommunionHour
    {
        private readonly string _connectionString;
        private readonly IWeek _week;
        public TrayCommunionHour(IConfiguration configuration, IWeek week)
        {
            _connectionString = configuration["ConnectionStrings:DefaultConnection"];
            _week = week;
        }

        public async Task AddSingleCommunionHour(int brotherId, int weekNumber, string hour)
        {
            var sql = "INSERT INTO communionHourOffice (userId, weekOfOffices, communionHour)" +
                "VALUES (@brotherId, @weekOfOffice, @communionHour)";
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            await connection.ExecuteAsync(sql, new { brotherId = brotherId, weekOfOffice = weekNumber, communionHour = hour });

        }

        public async Task AddCommunionHour(IEnumerable<CommunionOfficeAdded> offices)
        {
            int weekNumber = await _week.GetLastWeek();
            foreach (CommunionOfficeAdded office in offices)
            {
                var brotherId = office.IdBrother;
                foreach (var hour in office.CommunionHourOffices)
                {
                    await AddSingleCommunionHour(brotherId, weekNumber, hour);
                }
            }
        }

        public async Task AddSingleTrayHour(int brotherId, int weekNumber, string hour)
        {
            var sql = "INSERT INTO trayHourOffice (userId, weekOfOffice, trayHour)" +
                "VALUES (@brotherId, @weekOfOffice, @trayHour)";
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            await connection.ExecuteAsync(sql, new { brotherId = brotherId, weekOfOffice = weekNumber, trayHour = hour });
        }

        public async Task AddTrayHour(IEnumerable<TrayOfficeAdded> offices)
        {
            int weekNumber = await _week.GetLastWeek();
            foreach (TrayOfficeAdded office in offices)
            {
                var brotherId = office.IdBrother;
                foreach(var hour in office.TrayHourOffices)
                {
                    await AddSingleTrayHour(brotherId, weekNumber, hour);
                }
            }
        }

        public async Task<IEnumerable<CommunionOfficeAdded>> GetCommunionHours()
        {
            List<CommunionOfficeAdded> communionOffice = new List<CommunionOfficeAdded>();
            List<String> communionHour = new List<String>();

            using var connection = new SqlConnection(_connectionString);
            var sqlId = "SELECT DISTINCT userId IdBrother FROM communionHourOffice";
            var sqlTray = "SELECT communionHour from communionHourOffice WHERE userId = @userId";
            await connection.OpenAsync();
            var ids = await connection.QueryAsync<int>(sqlId);
            foreach (var id in ids)
            {
                communionHour = (await connection.QueryAsync<String>(sqlTray, new { userId = id })).ToList();
                if (communionHour.Any())
                {
                    communionOffice.Add(new CommunionOfficeAdded() { IdBrother = id, CommunionHourOffices = communionHour });
                }
            }
            return communionOffice;
        }

        public async Task<IEnumerable<CommunionOfficeAdded>> GetCommunionHours(int weekId)
        {
            List<CommunionOfficeAdded> communionOffice = new List<CommunionOfficeAdded>();
            List<String> communionHour = new List<String>();

            using var connection = new SqlConnection(_connectionString);
            var sqlId = "SELECT DISTINCT userId IdBrother FROM communionHourOffice";
            var sqlTray = "SELECT communionHour from communionHourOffice WHERE userId = @userId AND weekOfOffices = @weekId";
            await connection.OpenAsync();
            var ids = await connection.QueryAsync<int>(sqlId);
            foreach (var id in ids)
            {
                communionHour = (await connection.QueryAsync<String>(sqlTray, new { userId = id, weekId = weekId })).ToList();
                if (communionHour.Any())
                {
                    communionOffice.Add(new CommunionOfficeAdded() { IdBrother = id, CommunionHourOffices = communionHour, weekId = weekId });
                }
            }
            return communionOffice;
        }

        private async Task<IEnumerable<int>> GetUniqueBrotherIdFromTrayHourOffice()
        {
            var sqlId = "SELECT DISTINCT userId IdBrother FROM trayHourOffice";
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            return await connection.QueryAsync<int>(sqlId);
        }

        public async Task<IEnumerable<TrayOfficeAdded>> GetTrayHours()
        {
            List<TrayOfficeAdded> trayOffices = new List<TrayOfficeAdded>();
            List<String> trays = new List<String>();

            using var connection = new SqlConnection(_connectionString);
            var sqlTray = "SELECT trayHour from trayHourOffice WHERE userId = @userId";
            await connection.OpenAsync();
            var ids = await GetUniqueBrotherIdFromTrayHourOffice();
            foreach (var id in ids)
            {
                trays = (await connection.QueryAsync<String>(sqlTray, new { userId = id })).ToList();
                if(trays.Any())
                {
                    trayOffices.Add(new TrayOfficeAdded() { IdBrother = id, TrayHourOffices = trays });
                }
            }
            return trayOffices;
        }

        public async Task<IEnumerable<TrayOfficeAdded>> GetTrayHours(int weekId)
        {
            List<TrayOfficeAdded> trayOffices = new List<TrayOfficeAdded>();
            List<String> trays = new List<String>();
           
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var sql = "SELECT userId IdBrother, trayHour OfficeName from trayHourOffice WHERE weekOfOffice = @weekId";
            var result = (await connection.QueryAsync<SingleOfficeWithID>(sql, new { weekId })).ToList();

            var ids = result.Select(item => item.IdBrother).Distinct();
            foreach(var id in ids)
            {
                trays = result.Where(item => item.IdBrother == id).Select(tray => tray.OfficeName).ToList();
                trayOffices.Add(new TrayOfficeAdded { IdBrother = id, TrayHourOffices = trays, weekId = weekId });
            }

            return trayOffices;
        }
    }
}
