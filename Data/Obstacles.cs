using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtualDean.Models;
using VirtualDean.Models.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace VirtualDean.Data
{
    public class Obstacles : IObstacle
    {
        private readonly IWeek _week;
        private readonly ObstaclesDbContext _obstaclesDbContext;
        private readonly ObstacleConstDbContext _obstacleConstDbContext;
        private readonly ObstacleBetweenOfficesDbContext _obstacleBetweenOfficeDbContext;
        public Obstacles(IWeek week, ObstaclesDbContext obstaclesDbContext, ObstacleConstDbContext obstacleConstDbContext,
            ObstacleBetweenOfficesDbContext obstacleBetweenOfficesDbContext)
        {
            _week = week;
            _obstaclesDbContext = obstaclesDbContext;
            _obstacleConstDbContext = obstacleConstDbContext;
            _obstacleBetweenOfficeDbContext = obstacleBetweenOfficesDbContext;
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
            try
            {
                foreach (var obstacle in obstacles)
                {
                    obstacle.WeekOfOffices = weekNumber;
                    if (!await IsOBstacleInDb(obstacle))
                    {
                        await _obstaclesDbContext.AddAsync(obstacle);
                    }
                }
                await _obstaclesDbContext.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }

        private async Task<Boolean> IsOBstacleInDb(ObstaclesAdded obstacle)
        {
            return await _obstaclesDbContext.Obstacles.AnyAsync(item => item.Obstacle == obstacle.Obstacle && item.WeekOfOffices == obstacle.WeekOfOffices);
        }

        public async Task DeleteConstObstacle(ConstObstacleAdded obstacle)
        {
            _obstacleConstDbContext.Remove(obstacle);
            await _obstacleConstDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<ConstObstacleAdded>> GetAllConstObstacles()
        {
            return await _obstacleConstDbContext.ObstacleConst.ToListAsync();
        }

        public async Task<IEnumerable<string>> GetConstObstacleForBrother(int brotherId)
        {
            return await _obstacleConstDbContext.ObstacleConst.Where(obstacle => obstacle.BrotherId == brotherId).Select(obstacle => obstacle.ObstacleName).ToListAsync();
        }

        public async Task<ConstObstacleAdded> GetConstObstacle(int id)
        {
            return await _obstacleConstDbContext.ObstacleConst.FindAsync(id);
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
        public async Task<IEnumerable<ObstaclesList>> GetLastObstacleAdded()
        {
            int weekNumber = await _week.GetLastWeek();
            return await GetObstacles(weekNumber);
        }

        public async Task<IEnumerable<ConstObstacleWithBrotherData>> GetObstacleWithBrotherData(IEnumerable<BaseModel> brothers)
        {
            var constObstacle = await GetAllConstObstacles();
            List<ConstObstacleWithBrotherData> obstacleWithBrothersData = new List<ConstObstacleWithBrotherData>();
            foreach(var obstacle in constObstacle)
            {
                var brotherData = brothers.Where(bro => bro.Id == obstacle.BrotherId).Select(bro => new { Name = bro.Name, Surname = bro.Surname, BrotherId = bro.Id}).First();
                obstacleWithBrothersData.Add(new ConstObstacleWithBrotherData { Id = obstacle.Id, BrotherId = brotherData.BrotherId, Name = brotherData.Name, Surname = brotherData.Surname, ObstacleName = obstacle.ObstacleName });
            }
            return obstacleWithBrothersData;
        }

        public async Task EditConstObstacle(ConstObstacleAdded obstacle)
        {
            _obstacleConstDbContext.Entry(obstacle).State = EntityState.Modified;
            await _obstacleConstDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<ObstacleBetweenOffice>> GetObstacleBetweenOffices()
        {
            return await _obstacleBetweenOfficeDbContext.ObstacleBetweenOffices.ToListAsync();
        }

        public async Task AddObstacleBetweenOffices(ObstacleBetweenOffice obstacle)
        {
            await _obstacleBetweenOfficeDbContext.AddAsync(obstacle);
            await _obstacleBetweenOfficeDbContext.SaveChangesAsync();
        }

        public async Task EditObstacleBetweenOffices(ObstacleBetweenOffice obstacle)
        {
            _obstacleBetweenOfficeDbContext.Entry(obstacle).State = EntityState.Modified;
            await _obstacleBetweenOfficeDbContext.SaveChangesAsync();
        }

        public async Task DeleteObstacleBetweenOffices(ObstacleBetweenOffice obstacle)
        {
            _obstacleBetweenOfficeDbContext.Remove(obstacle);
            await _obstacleBetweenOfficeDbContext.SaveChangesAsync();
        }

        public async Task<ObstacleBetweenOffice> GetObstacleBetweenOffice(int id)
        {
            return await _obstacleBetweenOfficeDbContext.ObstacleBetweenOffices.FindAsync(id);
        }
    }
}
