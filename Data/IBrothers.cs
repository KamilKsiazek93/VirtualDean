﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtualDean.Models;

namespace VirtualDean.Data
{
    public interface IBrothers
    {
        Task<Brother> AddBrother(Brother brother);
        Task<IEnumerable<Brother>> GetBrothers();
        Task<Brother> GetBrother(int brotherId);
        Task<IEnumerable<BaseModel>> GetBaseBrothersModel();
        Task<IEnumerable<CantorResponse>> GetSingingBrothers();
        Task<IEnumerable<BaseModel>> GetBrothersForTray();
    }
}
