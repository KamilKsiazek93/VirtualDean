using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtualDean.Models;

namespace VirtualDean.Data
{
    public interface IBrothers
    {
        Task<IEnumerable<Brother>> GetBrothers();
        Task<Brother> GetBrother(int brotherId);
        Task<IEnumerable<BaseModel>> GetBaseBrothersModel();
        Task<IEnumerable<CantorResponse>> GetSingingBrothers();
        Task<IEnumerable<BaseModel>> GetBrothersForTray();
        Task<IEnumerable<BaseModel>> GetBrotherForLiturgistOffice();
        Task<Boolean> IsBrotherInDb(Brother brother);
        Task SaveBrother(Brother brother);
        Task DeleteBrother(Brother brother);
        Task EditBrother(Brother brother);
        Task<BaseModel> FindLoginBrother(LoginModel loginData);
    }
}
