using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VirtualDean.Models
{
    public class LiturgistResponse
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public List<KitchenOffices> KitchenOffices { get; set; }
        public List<Obstacles> Obstacles { get; set; }
        public List<Offices> Offices { get; set; }
    }
}
