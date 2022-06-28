﻿using System;
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
        Task<Boolean> IsScholaAlreadySet();
        Task<Boolean> IsKitchenOfficeAlreadySet();
        Task<Boolean> IsLiturgistOfficeAlreadySet();
        Task<Boolean> IsDeanOfficeAlreadySet(int weekNumber);
        Task AddLiturgistOffice(IEnumerable<Office> offices);
        Task AddDeanOffice(IEnumerable<Office> offices);
        Task<IEnumerable<string>> GetOfficesName();
        Task<IEnumerable<Office>> GetLastOffice();
        Task<Office> GetOfficeForBrother(int weekNumber, int brotherId);
        Task<Boolean> GetPipelineStatus(string name);
        Task UpdatePipelineStatus(string jobName, Boolean jobValue);
        Task<IEnumerable<OfficeNames>> GetOfficeNames(string adminName);
        Task<IEnumerable<OfficeNames>> GetOfficeNamesForObstacle();
        OfficeBrother GetOfficeForSingleBrother(IEnumerable<string> trays, IEnumerable<string> communions, Office otherOffices);
        OfficePrint GetOfficeForSingleBrotherPrint(IEnumerable<string> trays, IEnumerable<string> communions, Office otherOffices, BaseModel brother);
    }
}
