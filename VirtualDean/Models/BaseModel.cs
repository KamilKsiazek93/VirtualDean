﻿namespace VirtualDean.Models
{
    public class BaseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string StatusBrother { get; set; }
        public string? JwtToken { get; set; }
    }
}
