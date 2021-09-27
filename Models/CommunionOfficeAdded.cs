using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtualDean.Enties;

namespace VirtualDean.Models
{
    public class CommunionOfficeAdded
    {
        public int IdBrother { get; set; }
        public List<CommunionHour> CommunionHourOffices { get; set; }
    }
}
