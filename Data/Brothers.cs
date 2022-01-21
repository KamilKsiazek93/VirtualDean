using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using VirtualDean.Models;
using Dapper;
using VirtualDean.Models.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace VirtualDean.Data
{
    public class Brothers : IBrothers
    {
        private readonly BrotherDbContext _brotherContext;
        private readonly IAuth _auth;
        public Brothers(BrotherDbContext brotherContext, IAuth auth)
        {
            _brotherContext = brotherContext;
            _auth = auth;
        }

        public async Task DeleteBrother(Brother brother)
        {
             _brotherContext.Remove(brother);
            await _brotherContext.SaveChangesAsync();
        }

        public async Task EditBrother(Brother brother)
        {
            _brotherContext.Entry(brother).State = EntityState.Modified;
            await _brotherContext.SaveChangesAsync();
        }

        public async Task<BaseModel> FindLoginBrother(LoginModel loginData)
        {
            var hashedPassword = _auth.GetHashedPassword(loginData.Password);
            var brother = await _brotherContext.Brothers.Where(b => b.Email == loginData.Email 
            && b.PasswordHash == hashedPassword).FirstOrDefaultAsync();
            if(brother != null)
            {
                return new BaseModel { Id = brother.Id, Name = brother.Name, Surname = brother.Surname, StatusBrother = brother.StatusBrother };
            }
            return null;
        }

        public async Task<IEnumerable<BaseModel>> GetBaseBrothersModel()
        {
            return await _brotherContext.Brothers.Select(bro => new BaseModel() 
            { Id = bro.Id, Name = bro.Name, Surname = bro.Surname }).ToListAsync();
        }

        public async Task<Brother> GetBrother(int brotherId)
        {
            return await _brotherContext.Brothers.FindAsync(brotherId);
        }

        public async Task<IEnumerable<Brother>> GetBrothers()
        {
            return await _brotherContext.Brothers.ToListAsync();
        }

        public async Task<IEnumerable<BaseModel>> GetBrothersForTray()
        {
            return await _brotherContext.Brothers.Where(bro => !bro.isAcolit && !bro.isDiacon).ToListAsync();
        }

        public async Task<IEnumerable<CantorResponse>> GetSingingBrothers()
        {
            return await _brotherContext.Brothers.
                Where(bro => bro.isSinging).
                Select(bro => new CantorResponse()
            { Id = bro.Id, Name = bro.Name, Surname = bro.Surname, IsSinging = bro.isSinging}).ToListAsync();
        }

        public async Task<Boolean> IsBrotherInDb(Brother brother)
        {
            return await _brotherContext.Brothers.AnyAsync(bro => bro.Name == brother.Name && bro.Surname == brother.Surname);
        }

        public async Task SaveBrother(Brother brother)
        {
            try
            {
                brother.StatusBrother = "BRAT";
                brother.PasswordHash = _auth.GetHashedPassword(brother.Name + "123");
                await _brotherContext.Brothers.AddAsync(brother);
                await _brotherContext.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }
    }
}
