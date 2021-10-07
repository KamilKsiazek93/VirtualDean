using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VirtualDean.Models
{
    public class Brothers : BaseModel
    {
        public Boolean isSinging { get; set; }
        public Boolean isLector { get; set; }
        public Boolean isAcolit { get; set; }
        public Boolean isDiacon { get; set; }
    }
}
