using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VirtualDean.Models
{
    public class DeignateOfficeResponse
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public List<KitchenOffices> KitchenOffices { get; set; }
        public List<string> Obstacles { get; set; }
        public List<Offices> Offices { get; set; }
    }
}
