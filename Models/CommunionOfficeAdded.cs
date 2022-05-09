using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtualDean.Enties;

namespace VirtualDean.Models
{
    public class CommunionOfficeAdded
    {
        public int Id { get; set; }
        public int BrotherId { get; set; }
        public int WeekOfOffices { get; set; }
        public string CommunionHour { get; set; }
    }
}
