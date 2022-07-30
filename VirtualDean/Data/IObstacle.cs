using System.Collections.Generic;
using System.Threading.Tasks;
using VirtualDean.Models;

namespace VirtualDean.Data
{
    public interface IObstacle
    {
        Task AddObstacle(IEnumerable<ObstaclesAdded> obstacles);
        Task<IEnumerable<ObstaclesList>> GetObstacles(int weekId);
        Task<IEnumerable<ObstaclesList>> GetLastObstacleAdded();
        Task AddConstObstacle(ConstObstacleAdded obstacle);
        Task<IEnumerable<string>> GetConstObstacleForBrother(int brotherId);
        Task<IEnumerable<ConstObstacleAdded>> GetAllConstObstacles();
        Task<IEnumerable<ConstObstacleWithBrotherData>> GetObstacleWithBrotherData(IEnumerable<BaseModel> brothers);
        Task<ConstObstacleAdded> GetConstObstacle(int id);
        Task DeleteConstObstacle(ConstObstacleAdded obstacle);
        Task EditConstObstacle(ConstObstacleAdded obstacle);
        Task<IEnumerable<ObstacleBetweenOffice>> GetObstacleBetweenOffices();
        Task AddObstacleBetweenOffices(ObstacleBetweenOffice obstacle);
        Task EditObstacleBetweenOffices(ObstacleBetweenOffice obstacle);
        Task DeleteObstacleBetweenOffices(ObstacleBetweenOffice obstacle);
        Task<ObstacleBetweenOffice> GetObstacleBetweenOffice(int id);
    }
}
