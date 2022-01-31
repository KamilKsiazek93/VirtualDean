using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VirtualDean.Models
{
    public class ObstacleBetweenOffice
    {
        public int Id { get; set; }
        public string OfficeName { get; set; }
        public string OfficeConnected { get; set; }
    }
}
