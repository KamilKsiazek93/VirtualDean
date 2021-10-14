using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using VirtualDean.Models;
using Dapper;

namespace VirtualDean.Data
{
    public class Brothers : IBrothers
    {
        private readonly string _connectionString;
        public Brothers(IConfiguration configuration)
        {
            _connectionString = configuration["ConnectionStrings:DefaultConnection"];
        }

        public async Task<Brother> AddBrother(Brother brother)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            DateTime today = DateTime.Today;

            var sql = @"INSERT INTO dbo.brothers (name, surname, precedency, isSinging, isLector, isAcolit, isDiacon)
                OUTPUT INSERTED.[id]
                VALUES (@Name, @Surname, @Precedency, @isSinging, @isLector, @isAcolit, @isDiacon)";

            var brotherId = connection.QuerySingle<int>(sql, new { Name = brother.Name, Surname = brother.Surname, Precedency = today,
            IsSinging = brother.isSinging, IsLector = brother.isLector, IsAcolit = brother.isAcolit, IsDiacon = brother.isDiacon});

            return await GetBrother(brotherId);
        }

        public async Task<Brother> GetBrother(int brotherId)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var sql = "SELECT * FROM brothers WHERE id = @brotherId";

            return await connection.QueryFirstOrDefaultAsync<Brother>(sql, new { brotherId = brotherId });
        }

        public async Task<IEnumerable<Brother>> GetBrothers()
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var sql = "SELECT * FROM brothers";

            return (await connection.QueryAsync<Brother>(sql)).ToList();
        }
    }
}
