using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtualDean.Enties;

namespace VirtualDean.Models
{
    public class DeanOfficeAdded
    {
        public int IdBrother { get; set; }
        public DeanOffice OfficeName { get; set; }
    }
}
