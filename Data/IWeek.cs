using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VirtualDean.Data
{
    public interface IWeek
    {
        Task IncrementWeek(int weekActual);
        Task<int> GetLastWeek();
    }
}
