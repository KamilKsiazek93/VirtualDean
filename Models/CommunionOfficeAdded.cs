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
        public List<String> CommunionHourOffices { get; set; }
        public int? weekId { get; set; }
    }
}
