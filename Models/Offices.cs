using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtualDean.Enties;

namespace VirtualDean.Models
{
    public class Offices
    {
        public CantorOffice? Schola { get; set; }
        public CantorGregOffice? Gregorian { get; set; }
        public LiturgyOffice? Liturgy { get; set; }
        public TrayHour? Tray { get; set; }
        public CommunionHour? Communion { get; set; }
    }
}
