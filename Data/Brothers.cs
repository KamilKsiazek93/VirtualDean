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
        public Brothers(BrotherDbContext brotherContext)
        {
            _brotherContext = brotherContext;
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
