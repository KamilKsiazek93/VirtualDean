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
        Task AddKitchenOffices(IEnumerable<KitchenOffices> kitchenOffices);
        Task AddBrothersForSchola(IEnumerable<Office> schola);
        Task AddLiturgistOffice(IEnumerable<Office> offices);
        Task<IEnumerable<string>> GetOfficesName();
        Task<IEnumerable<Office>> GetLastOffice();
        Task<Office> GetLastOfficeForBrother(int brotherId);
    }
}
