﻿using Microsoft.Extensions.Configuration;
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
            List<CommunionOfficeAdded> communionOffice = new List<CommunionOfficeAdded>();
            List<String> communionHour = new List<String>();

            using var connection = new SqlConnection(_connectionString);
            var sql = "SELECT DISTINCT userId IdBrother, communionHour OfficeName FROM communionHourOffice";
            await connection.OpenAsync();
            var result = (await connection.QueryAsync<SingleOfficeWithID>(sql)).ToList();
            var ids = result.Select(item => item.IdBrother).Distinct();
            foreach (var id in ids)
            {
                communionHour = (result.Where(item => item.IdBrother == id).Select(item => item.OfficeName)).ToList();
                communionOffice.Add(new CommunionOfficeAdded() { IdBrother = id, CommunionHourOffices = communionHour });
            }
            return communionOffice;
        }

        public async Task<IEnumerable<CommunionOfficeAdded>> GetCommunionHours(int weekId)
        {
            List<CommunionOfficeAdded> communionOffice = new List<CommunionOfficeAdded>();
            List<String> communionHour = new List<String>();

            using var connection = new SqlConnection(_connectionString);
            var sql = "SELECT userId IdBrother, communionHour OfficeName FROM communionHourOffice WHERE weekOfOffices = @weekId";
            await connection.OpenAsync();
            var result = (await connection.QueryAsync<SingleOfficeWithID>(sql, new { weekId })).ToList();
            var ids = result.Select(item => item.IdBrother).Distinct();
            foreach (var id in ids)
            {
                communionHour = (result.Where(item => item.IdBrother == id).Select(item => item.OfficeName)).ToList();
                communionOffice.Add(new CommunionOfficeAdded() { IdBrother = id, CommunionHourOffices = communionHour, weekId = weekId });
            }
            return communionOffice;
        }
        public async Task<IEnumerable<TrayOfficeAdded>> GetTrayHours()
        {
            return await _trayContext.TrayHourOffice.ToListAsync();
        }

        public async Task<IEnumerable<TrayOfficeAdded>> GetTrayHours(int weekId)
        {
            return await _trayContext.TrayHourOffice.Where(tray => tray.WeekOfOffices == weekId).ToListAsync();
        }
    }
}
