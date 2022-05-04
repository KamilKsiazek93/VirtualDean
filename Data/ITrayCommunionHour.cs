using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtualDean.Models;

namespace VirtualDean.Data
{
    public interface ITrayCommunionHour
    {
        Task AddTrayHour(IEnumerable<TrayOfficeAdded> offices);
        Task<IEnumerable<TrayOfficeAdded>> GetTrayHours();
        Task<IEnumerable<TrayOfficeAdded>> GetTrayHours(int weekId);
        Task<IEnumerable<LastTrayOfficeList>> GetLastTrayHour();

        Task AddCommunionHour(IEnumerable<CommunionOfficeAdded> offices);
        Task<IEnumerable<CommunionOfficeAdded>> GetCommunionHours();
        Task<IEnumerable<CommunionOfficeAdded>> GetCommunionHours(int weekId);
    }
}
