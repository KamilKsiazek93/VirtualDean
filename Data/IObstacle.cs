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
        Task<IEnumerable<ObstaclesAdded>> GetObstacles(int weekId);
        Task AddConstObstacle(ConstObstacleAdded obstacle);
        Task<IEnumerable<string>> GetConstObstacle(int brotherId);
    }
}
