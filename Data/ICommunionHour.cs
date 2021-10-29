﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtualDean.Models;

namespace VirtualDean.Data
{
    public interface ICommunionHour
    {
        Task AddCommunionHour(IEnumerable<CommunionOfficeAdded> offices);
        Task<IEnumerable<CommunionOfficeAdded>> GetCommunionHours();
        Task<IEnumerable<CommunionOfficeAdded>> GetCommunionHours(int weekId);
    }
}