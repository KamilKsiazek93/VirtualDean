using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtualDean.Models;

namespace VirtualDean.Data
{
    public class Kitchen : IKitchen
    {
        public async Task<KitchenOffices> AddKitchenOffices()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<KitchenOffices>> GetKitchenOffices()
        {
            throw new NotImplementedException();
        }
    }
}
