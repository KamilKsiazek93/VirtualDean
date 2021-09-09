using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VirtualDean.Models
{
    public class LiturgistResponseOffice
    {
        public string name { get; set; }
        public string surname { get; set; }
        public List<KitchenOffices> KitchenOffices { get; set; }
        public List<string> Obstacles { get; set; }
        public List<Offices> Offices { get; set; }
    }
}
