using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtualDean.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace VirtualDean.Data
{
    public class CommunionHour : ICommunionHour
    {
        private readonly string _connectionString;
        public CommunionHour(IConfiguration configuration)
        {
            _connectionString = configuration["ConnectionStrings:DefaultConnection"];
        }

        public async Task AddCommunionHour(IEnumerable<CommunionOfficeAdded> offices)
        {
            int weekNumber = await GetWeekNumber();
            foreach (CommunionOfficeAdded office in offices)
            {
                var brotherId = office.IdBrother;
                var sql = "INSERT INTO communionHourOffice (userId, weekOfOffices, communionHour)" +
                    "VALUES (@brotherId, @weekOfOffice, @communionHour)";
                foreach (var hour in office.CommunionHourOffices)
                {
                    using var connection = new SqlConnection(_connectionString);
                    await connection.OpenAsync();
                    await connection.ExecuteAsync(sql, new { brotherId = brotherId, weekOfOffice = weekNumber, communionHour = hour.ToString() });
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

        private async Task<int> GetWeekNumber()
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "SELECT TOP 1 weekNumber FROM weeksNumber ORDER BY weekNumber DESC";
            await connection.OpenAsync();
            return await connection.QueryFirstAsync<int>(sql);
        }
    }
}
