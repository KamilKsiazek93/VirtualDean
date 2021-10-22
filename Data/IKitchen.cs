using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtualDean.Models;

namespace VirtualDean.Data
{
    public interface IKitchen
    {
        Task<IEnumerable<AllKitchenOffices>> GetKitchenOffices();
        Task<IEnumerable<KitchenOffices>> GetKitchenOffices(int weekId);
        Task AddKitchenOffices(IEnumerable<KitchenOffices> kitchenOffices);
    }
}
