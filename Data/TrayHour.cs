using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtualDean.Models;

namespace VirtualDean.Data
{
    public class TrayHour : ITrayHour
    {
        public async Task AddTrayHour(IEnumerable<TrayOfficeAdded> offices)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TrayOfficeAdded>> GetTrayHours()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TrayOfficeAdded>> GetTrayHours(int weekId)
        {
            throw new NotImplementedException();
        }
    }
}
