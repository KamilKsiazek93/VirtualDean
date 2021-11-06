using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using VirtualDean.Models;

namespace VirtualDean.Data
{
    public class OfficesManager : IOfficesManager
    {
        private readonly string _connectionString;
        private readonly IWeek _week;
        public OfficesManager(IConfiguration configuration, IWeek week)
        {
            _connectionString = configuration["ConnectionStrings:DefaultConnection"];
            _week = week;
        }
        public async Task AddKitchenOffices(IEnumerable<KitchenOffices> kitchenOffices)
        {
            int weekNumber = await _week.GetLastWeek();
            foreach (KitchenOffices office in kitchenOffices)
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();
                var sql = "INSERT INTO kitchenOffice (userId, weekOfOffices, saturdayOffices, sundayOffices)" +
                "VaLUES (@userId, @weekOfOffices, @saturdayOffices, @sundayOffices)";
                await connection.ExecuteAsync(sql, new {userId = office.BrotherId, weekOfOffices = weekNumber, 
                    saturdayOffices = office.SaturdayOffice, sundayOffices = office.SundayOffice });
            }
        }

        public async Task<IEnumerable<KitchenOffices>> GetKitchenOffices()
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "SELECT userId brotherId, weekOfOffices WeekOfOffices, saturdayOffices SaturdayOffice, sundayOffices SundayOffice FROM kitchenOffice";
            return (await connection.QueryAsync<KitchenOffices>(sql)).ToList();
        }

        public async Task<IEnumerable<KitchenOffices>> GetKitchenOffices(int weekId)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "SELECT userId brotherId, saturdayOffices SaturdayOffice, sundayOffices SundayOffice FROM kitchenOffice" +
                " WHERE weekOfOffices = @weekId";
            return (await connection.QueryAsync<KitchenOffices>(sql, new { weekId = weekId })).ToList();
        }
    }
}
