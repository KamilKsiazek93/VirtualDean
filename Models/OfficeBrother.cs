using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VirtualDean.Models
{
    public class OfficeBrother
    {
        public int BrotherId { get; set; }
        public string CantorOffice { get; set; }
        public string LiturgistOffice { get; set; }
        public string DeanOffice { get; set; }
        public List<string> Tray { get; set; }
        public List<string> Communion { get; set; }
    }
}
