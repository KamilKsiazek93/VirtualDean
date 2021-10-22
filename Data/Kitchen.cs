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
    public class Kitchen : IKitchen
    {
        private readonly string _connectionString;
        public Kitchen(IConfiguration configuration)
        {
            _connectionString = configuration["ConnectionStrings:DefaultConnection"];
        }
        public async Task AddKitchenOffices(IEnumerable<KitchenOffices> kitchenOffices)
        {
            int weekNumber = await GetWeekNumber();
            //int weekNumber = 1;
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

        private async Task<int> GetWeekNumber()
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "SELECT TOP 1 weekNumber FROM weeksNumber ORDER BY weekNumber DESC";
            await connection.OpenAsync();
            return await connection.QueryFirstAsync<int>(sql);
        }

        public async Task<IEnumerable<AllKitchenOffices>> GetKitchenOffices()
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "SELECT userId brotherId, weekOfOffices WeekOfOffices, saturdayOffices SaturdayOffice, sundayOffices SundayOffice FROM kitchenOffice";
            return (await connection.QueryAsync<AllKitchenOffices>(sql)).ToList();
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
