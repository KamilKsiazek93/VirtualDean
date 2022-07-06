using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VirtualDean.Models;

namespace VirtualDean.Data
{
    public interface IBrothers
    {
        Task SetupBrothersTable();
        Task<IEnumerable<Brother>> GetBrothers();
        Task<Brother> GetBrother(int brotherId);
        Task<IEnumerable<BaseModel>> GetBaseBrothersModel();
        Task<IEnumerable<CantorResponse>> GetSingingBrothers();
        Task<IEnumerable<BaseModel>> GetBrothersForTray();
        Task<IEnumerable<BaseModel>> GetBrothersForCommunion();
        Task<IEnumerable<BaseBrotherForLiturgistOffice>> GetBrotherForLiturgistOffice();
        Task<Boolean> IsBrotherInDb(Brother brother);
        Task SaveBrother(Brother brother);
        Task DeleteBrother(Brother brother);
        Task EditBrother(Brother brother);
        Task<BaseModel> FindLoginBrother(LoginModel loginData);
        BaseModel GetAuthenticatedBrother(BaseModel brother);
        Task UpdatePassword(PasswordUpdate passwordUpdate, Brother brother);
        string GetHashedPassword(string currentPassword);
    }
}
