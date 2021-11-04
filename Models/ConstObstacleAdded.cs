using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VirtualDean.Models
{
    public class ConstObstacleAdded
    {
        public int IdBrother { get; set; }
        public List<string> Obstacles { get; set; }
    }
}
