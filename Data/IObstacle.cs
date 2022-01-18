using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtualDean.Models;

namespace VirtualDean.Data
{
    public interface IObstacle
    {
        Task AddObstacle(IEnumerable<ObstaclesAdded> obstacles);
        Task<IEnumerable<ObstaclesList>> GetObstacles(int weekId);
        Task AddConstObstacle(ConstObstacleAdded obstacle);
        Task<IEnumerable<string>> GetConstObstacleForBrother(int brotherId);
        Task<IEnumerable<ConstObstacleAdded>> GetAllConstObstacles();
        Task<IEnumerable<ConstObstacleWithBrotherData>> GetObstacleWithBrotherData(IEnumerable<BaseModel> brothers);
        Task<ConstObstacleAdded> GetConstObstacle(int id);
        Task DeleteConstObstacle(ConstObstacleAdded obstacle);
        Task EditConstObstacle(ConstObstacleAdded obstacle);
    }
}
