using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VirtualDean.Models;

namespace VirtualDean.Data
{
    public interface ITrayRepository
    {
        Task AddTrayHour(IEnumerable<TrayOfficeAdded> offices);
        Task<IEnumerable<TrayOfficeAdded>> GetTrayHours();
        Task<IEnumerable<TrayOfficeAdded>> GetTrayHours(int weekId);
        Task<IEnumerable<LastTrayOfficeList>> GetLastTrayHour();
        Task<IEnumerable<string>> GetTrayHour(int weekNumber, int idBrother);
        Task<Boolean> IsTrayAlreadySet();
        Task<IEnumerable<string>> GetHoursForTray();
    }
}
