using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtualDean.Models;

namespace VirtualDean.Data
{
    public class Obstacles : IObstacle
    {
        public async Task AddObstacle(IEnumerable<ObstaclesAdded> obstacles)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ObstaclesAdded>> GetObstacles(int weekId)
        {
            throw new NotImplementedException();
        }
    }
}
