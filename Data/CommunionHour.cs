using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtualDean.Models;

namespace VirtualDean.Data
{
    public class CommunionHour : ICommunionHour
    {
        public Task AddCommunionHour(IEnumerable<CommunionOfficeAdded> offices)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CommunionOfficeAdded>> GetCommunionHours()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CommunionOfficeAdded>> GetCommunionHours(int weekId)
        {
            throw new NotImplementedException();
        }
    }
}
