using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtualDean.Models;

namespace VirtualDean.Data
{
    public interface IOfficesManager
    {
        Task<IEnumerable<KitchenOffices>> GetKitchenOffices();
        Task<IEnumerable<KitchenOffices>> GetKitchenOffices(int weekId);
        Task AddKitchenOffices(IEnumerable<KitchenOfficeAdded> kitchenOffices);
        Task AddBrothersForSchola(IEnumerable<CantorOfficeAdded> schola);
        Task<IEnumerable<string>> GetOfficesName();
    }
}
