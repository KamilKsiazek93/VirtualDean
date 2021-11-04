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

        public async Task AddConstObstacle(ConstObstacleAdded obstacles)
        {
            var sql = "INSERT INTO obstacleConst (userId, obstacleName) VALUES (@userId, @obstacleName)";
            var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            foreach(var obstacle in obstacles.Obstacles)
            {
                await connection.ExecuteAsync(sql, new { userId = obstacles.IdBrother , obstacleName = obstacle});
            }
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

        public async Task<IEnumerable<string>> GetConstObstacle(int brotherId)
        {
            var sql = "SELECT obstacleName from obstacleConst WHERE userId = @brotherId";
            var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            return (await connection.QueryAsync<string>(sql, new { brotherId = brotherId })).ToList(); 
        }

        public async Task<IEnumerable<ObstaclesAdded>> GetObstacles(int weekId)
        {
            List<ObstaclesAdded> obstacles = new List<ObstaclesAdded>();
            List<String> obstacle = new List<String>();

            using var connection = new SqlConnection(_connectionString);
            var sqlId = "SELECT DISTINCT userId IdBrother FROM obstacles";
            var sqlObstacles = "SELECT obstacle from obstacles WHERE userId = @userId AND weekOfOffices = @weekId AND obstacle IS NOT NULL";
            var sqlWholeWeek = "SELECT wholeWeek from obstacles WHERE userId = @userId AND weekOfOffices = @weekId";

            await connection.OpenAsync();
            var ids = await connection.QueryAsync<int>(sqlId);
            foreach (var id in ids)
            {
                obstacle = (await connection.QueryAsync<String>(sqlObstacles, new { userId = id, weekId = weekId })).ToList();
                if (obstacle.Any())
                {
                    obstacles.Add(new ObstaclesAdded() { IdBrother = id, Obstacles = obstacle, weekId = weekId });
                }
                else
                {
                    var wholeWeek = await connection.QueryFirstOrDefaultAsync<Boolean>(sqlWholeWeek, new { userId = id, weekId = weekId });
                    if(wholeWeek)
                    {
                        obstacles.Add(new ObstaclesAdded() { IdBrother = id, WholeWeek = wholeWeek, weekId = weekId });
                    }
                }
            }
            return obstacles;
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
