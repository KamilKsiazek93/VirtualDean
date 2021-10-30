using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtualDean.Enties;

namespace VirtualDean.Models
{
    public class TrayOfficeAdded
    {
        public int IdBrother { get; set; }
        public List<String> TrayHourOffices { get; set; }
        public int? weekId { get; set; }
    }
}
