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
    public class TrayHour : ITrayHour
    {
        private readonly string _connectionString;
        public TrayHour(IConfiguration configuration)
        {
            _connectionString = configuration["ConnectionStrings:DefaultConnection"];
        }
        public async Task AddTrayHour(IEnumerable<TrayOfficeAdded> offices)
        {
            int weekNumber = await GetWeekNumber();
            foreach (TrayOfficeAdded office in offices)
            {
                var brotherId = office.IdBrother;
                var sql = "INSERT INTO trayHourOffice (userId, weekOfOffice, trayHour)" +
                    "VALUES (@brotherId, @weekOfOffice, @trayHour)";
                foreach(var hour in office.TrayHourOffices)
                {
                    using var connection = new SqlConnection(_connectionString);
                    await connection.OpenAsync();
                    await connection.ExecuteAsync(sql, new { brotherId = brotherId, weekOfOffice = weekNumber, trayHour = hour.ToString()});
                }
            }
        }

        public async Task<IEnumerable<TrayOfficeAdded>> GetTrayHours()
        {
            List<TrayOfficeAdded> trayOffices = new List<TrayOfficeAdded>();
            List<String> trays = new List<String>();

            using var connection = new SqlConnection(_connectionString);
            var sqlId = "SELECT userId IdBrother FROM trayHourOffice";
            var sqlTray = "SELECT trayHour from trayHourOffice WHERE userId = @userId";
            await connection.OpenAsync();
            var ids = await connection.QueryAsync<int>(sqlId);
            foreach(var id in ids)
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
            var sqlId = "SELECT userId IdBrother FROM trayHourOffice";
            var sqlTray = "SELECT trayHour from trayHourOffice WHERE userId = @userId AND weekOfOffice = @weekId";
            await connection.OpenAsync();
            var ids = await connection.QueryAsync<int>(sqlId);
            foreach (var id in ids)
            {
                trays = (await connection.QueryAsync<String>(sqlTray, new { userId = id, weekId = weekId })).ToList();
                if(trays.Any())
                {
                    trayOffices.Add(new TrayOfficeAdded() { IdBrother = id, TrayHourOffices = trays, weekId = weekId });
                }
            }
            return trayOffices;
        }

        private async Task<int> GetWeekNumber()
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "SELECT TOP 1 weekNumber FROM weeksNumber ORDER BY weekNumber DESC";
            await connection.OpenAsync();
            return await connection.QueryFirstAsync<int>(sql);
        }
    }
}
