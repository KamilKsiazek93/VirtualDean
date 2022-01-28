﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VirtualDean.Models
{
    public class DesignateOfficeResponse
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public List<KitchenOffices> KitchenOffices { get; set; }
        public List<string> Obstacles { get; set; }
        public List<Office> Offices { get; set; }
    }
}
