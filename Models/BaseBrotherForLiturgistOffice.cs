using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VirtualDean.Models
{
    public class BaseBrotherForLiturgistOffice : BaseModel
    {
        public Boolean IsAcolit { get; set; }
        public Boolean IsLector { get; set; }
    }
}
