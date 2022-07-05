using System;

namespace VirtualDean.Models
{
    public class BaseBrotherForLiturgistOffice : BaseModel
    {
        public Boolean IsAcolit { get; set; }
        public Boolean IsLector { get; set; }
    }
}
