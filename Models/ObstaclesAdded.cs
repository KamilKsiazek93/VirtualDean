using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VirtualDean.Models
{
    public class ObstaclesAdded
    {
        public int IdBrother { get; set; }
        public Boolean WholeWeek { get; set; }
        public int? weekId { get; set; }
        public List<string> Obstacles { get; set; }
    }
}
