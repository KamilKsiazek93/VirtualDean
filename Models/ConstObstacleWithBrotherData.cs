using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VirtualDean.Models
{
    public class ConstObstacleWithBrotherData : BaseModel
    {
        public int BrotherId { get; set; }
        public string ObstacleName { get; set; }
    }
}
