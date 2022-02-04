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
        private readonly OfficeDbContext _officeDbContext;
        private readonly KitchenOfficeDbContext _kitchenContext;
        private readonly IWeek _week;
        public OfficesManager(IConfiguration configuration, IWeek week, OfficeNameDbContext officeNameDbContext,
            OfficeDbContext officeDbContext, KitchenOfficeDbContext kitchenContext)
        {
            _connectionString = configuration["ConnectionStrings:DefaultConnection"];
            _officeNameContext = officeNameDbContext;
            _week = week;
            _officeDbContext = officeDbContext;
            _kitchenContext = kitchenContext;
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

        public async Task AddKitchenOffices(IEnumerable<KitchenOffices> kitchenOffices)
        {
            int weekOfOffice = await _week.GetLastWeek();
            foreach(var office in kitchenOffices)
            {
                office.WeekOfOffices = weekOfOffice;
            }

            await _kitchenContext.AddRangeAsync(kitchenOffices);
            await _kitchenContext.SaveChangesAsync();
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

        public async Task AddBrothersForSchola(IEnumerable<Office> offices)
        {
            int weekNumber = await _week.GetLastWeek();
            foreach(var office in offices)
            {
                office.WeekOfOffices = weekNumber;
                
                await _officeDbContext.AddAsync(office);
            }
            await _officeDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<string>> GetOfficesName()
        {
            return await _officeNameContext.OfficeNames.Select(i => i.OfficeName).ToListAsync();
        }
    }
}
