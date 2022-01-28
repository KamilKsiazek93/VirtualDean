using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using VirtualDean.Models;
using VirtualDean.Enties;
using VirtualDean.Models.DatabaseContext;

namespace VirtualDean.Data
{
    public class OfficesManager : IOfficesManager
    {
        private readonly string _connectionString;
        private readonly OfficeNameDbContext _officeNameContext;
        private readonly IWeek _week;
        public OfficesManager(IConfiguration configuration, IWeek week, OfficeNameDbContext officeNameDbContext)
        {
            _connectionString = configuration["ConnectionStrings:DefaultConnection"];
            _officeNameContext = officeNameDbContext;
            _week = week;
        }

        public async Task AddSingleSaturdayKitchenOffice(int brotherId, int weekNumber, string officeName)
        {
            var sql = "INSERT INTO kitchenOffice (userId, weekOfOffices, saturdayOffices)" +
                "VALUES (@brotherId, @weekOfOffices, @saturdayOffices)";
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            await connection.ExecuteAsync(sql, new { brotherId , weekOfOffices = weekNumber , saturdayOffices = officeName });
        }

        public async Task AddSingleSundayKitchenOffice(int brotherId, int weekNumber, string officeName)
        {
            var sql = "INSERT INTO kitchenOffice (userId, weekOfOffices, sundayOffices)" +
                "VALUES (@brotherId, @weekOfOffices, @sundayOffices)";
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            await connection.ExecuteAsync(sql, new { brotherId = brotherId, weekOfOffices = weekNumber, sundayOffices = officeName });
        }

        public async Task AddKitchenOffices(IEnumerable<KitchenOfficeAdded> kitchenOffices)
        {
            int weekNumber = await _week.GetLastWeek();
            foreach (KitchenOfficeAdded office in kitchenOffices)
            {
                if(office.Day.ToUpper().Equals("SATURDAY"))
                {
                    await AddSingleSaturdayKitchenOffice(office.BrotherId, weekNumber, office.OfficeName);
                }
                else if(office.Day.ToUpper().Equals("SUNDAY"))
                {
                    await AddSingleSundayKitchenOffice(office.BrotherId, weekNumber, office.OfficeName);
                }
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

        public async Task AddBrothersForSchola(IEnumerable<CantorOfficeAdded> schola)
        {
            int weekNumber = await _week.GetLastWeek();
            foreach(var brother in schola)
            {
                await AddSingleScholaOffice(brother.IdBrother, brother.OfficeName, weekNumber);
            }
        }

        private async Task AddSingleScholaOffice(int idBrother, string officeName, int weekNumber)
        {
            var sql = "INSERT INTO offices (userId, weekOfOffices, officeName)" +
                "VALUES (@idBrother, @weekNumber, @officeName)";
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            await connection.ExecuteAsync(sql, new { idBrother, weekNumber, officeName });
        }

        public async Task<IEnumerable<string>> GetOfficesName()
        {
            return await _officeNameContext.OfficeNames.Select(i => i.OfficeName).ToListAsync();
        }
    }
}
