using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtualDean.Models;
using Dapper;
using VirtualDean.Models.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace VirtualDean.Data
{
    public class Obstacles : IObstacle
    {
        private readonly string _connectionString;
        private readonly IWeek _week;
        private readonly ObstaclesDbContext _obstaclesDbContext;
        private readonly ObstacleConstDbContext _obstacleConstDbContext;
        public Obstacles(IConfiguration configuration, IWeek week, ObstaclesDbContext obstaclesDbContext, ObstacleConstDbContext obstacleConstDbContext)
        {
            _connectionString = configuration["ConnectionStrings:DefaultConnection"];
            _week = week;
            _obstaclesDbContext = obstaclesDbContext;
            _obstacleConstDbContext = obstacleConstDbContext;
        }

        public async Task AddConstObstacle(ConstObstacleAdded obstacles)
        {
            try
            {
                await _obstacleConstDbContext.AddAsync(obstacles);
                await _obstacleConstDbContext.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task AddObstacle(IEnumerable<ObstaclesAdded> obstacles)
        {
            int weekNumber = await _week.GetLastWeek();
            foreach(var obstacle in obstacles)
            {
                obstacle.WeekOfOffices = weekNumber;
            }

            try
            {
                await _obstaclesDbContext.AddRangeAsync(obstacles);
                await _obstaclesDbContext.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task<IEnumerable<string>> GetConstObstacle(int brotherId)
        {
            return await _obstacleConstDbContext.ObstacleConst.Where(obstacle => obstacle.BrotherId == brotherId).Select(obstacle => obstacle.ObstacleName).ToListAsync();
        }

        public async Task<IEnumerable<ObstaclesList>> GetObstacles(int weekId)
        {
            List<ObstaclesList> obstaclesFromDB = new List<ObstaclesList>();
            var obstacles =  await _obstaclesDbContext.Obstacles.Where(o => o.WeekOfOffices == weekId).ToListAsync();
            var ids = obstacles.Select(o => o.BrotherId).Distinct();
            foreach(var id in ids)
            {
                obstaclesFromDB.Add(new ObstaclesList { BrotherId = id, Obstacles = obstacles.Where(o => o.BrotherId == id).Select(o => o.Obstacle).ToList() });
            }

            return obstaclesFromDB;
        }
    }
}
