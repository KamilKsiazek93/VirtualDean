using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VirtualDean.Models
{
    public class ObstaclesAdded
    {
        public int Id { get; set; }
        public int BrotherId { get; set; }
        public int WeekOfOffices { get; set; }
        public string Obstacle { get; set; }
    }
}
