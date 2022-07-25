using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VirtualDean.Models;

namespace VirtualDean.Data
{
    public interface ITrayCommunionHour
    {
       
        Task AddCommunionHour(IEnumerable<CommunionOfficeAdded> offices);
        Task<IEnumerable<CommunionOfficeAdded>> GetCommunionHours();
        Task<IEnumerable<CommunionOfficeAdded>> GetCommunionHours(int weekId);
        Task<IEnumerable<string>> GetCommunionHour(int weekNumber, int idBrother);
        Task<IEnumerable<string>> GetHoursForCommunion();
    }
}
