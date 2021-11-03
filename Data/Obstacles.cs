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
    public class Obstacles : IObstacle
    {
        private readonly string _connectionString;
        public Obstacles(IConfiguration configuration)
        {
            _connectionString = configuration["ConnectionStrings:DefaultConnection"];
        }
        public async Task AddObstacle(IEnumerable<ObstaclesAdded> obstacles)
        {
            int weekNumber = await GetWeekNumber();
            foreach(ObstaclesAdded obstacle in obstacles)
            {
                var brotherId = obstacle.IdBrother;
                if(obstacle.WholeWeek)
                {
                    var sqlWhole = "INSERT INTO obstacles (userId, weekOfOffices, wholeWeek)" +
                    "VALUES (@brotherId, @weekOfOffice, @wholeWeek)";
                    using var connection = new SqlConnection(_connectionString);
                    await connection.OpenAsync();
                    await connection.ExecuteAsync(sqlWhole, new { brotherId = brotherId, weekOfOffice = weekNumber, wholeWeek = obstacle.WholeWeek });
                }
                else
                {
                    var sql = "INSERT INTO obstacles (userId, weekOfOffices, obstacle)" +
                    "VALUES (@brotherId, @weekOfOffice, @obstacle)";
                    foreach (var singleObstacle in obstacle.Obstacles)
                    {
                        using var connection = new SqlConnection(_connectionString);
                        await connection.OpenAsync();
                        await connection.ExecuteAsync(sql, new { brotherId = brotherId, @weekOfOffice = weekNumber, obstacle = singleObstacle });
                    }
                }
            }
        }

        public async Task<IEnumerable<ObstaclesAdded>> GetObstacles(int weekId)
        {
            throw new NotImplementedException();
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
