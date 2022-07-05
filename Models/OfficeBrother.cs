using System.Collections.Generic;

namespace VirtualDean.Models
{
    public class OfficeBrother
    {
        public int BrotherId { get; set; }
        public string CantorOffice { get; set; }
        public string LiturgistOffice { get; set; }
        public string DeanOffice { get; set; }
        public IEnumerable<string> Tray { get; set; }
        public IEnumerable<string> Communion { get; set; }
    }
}
