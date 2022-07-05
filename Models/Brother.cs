using System;

namespace VirtualDean.Models
{
    public class Brother : BaseModel
    {
        public DateTime Precedency { get; set; }
        public Boolean IsSinging { get; set; }
        public Boolean IsLector { get; set; }
        public Boolean IsAcolit { get; set; }
        public Boolean IsDiacon { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
    }
}
