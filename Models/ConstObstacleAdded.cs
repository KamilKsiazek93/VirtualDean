using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VirtualDean.Models
{
    public class ConstObstacleAdded
    {
        public int Id { get; set; }
        public int BrotherId { get; set; }
        public string ObstacleName { get; set; }
    }
}
