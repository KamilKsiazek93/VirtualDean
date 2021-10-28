using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtualDean.Models;

namespace VirtualDean.Data
{
    public interface ITrayHour
    {
        Task AddTrayHour(IEnumerable<TrayOfficeAdded> offices);
        Task<IEnumerable<TrayOfficeAdded>> GetTrayHours();
        Task<IEnumerable<TrayOfficeAdded>> GetTrayHours(int weekId);
    }
}
