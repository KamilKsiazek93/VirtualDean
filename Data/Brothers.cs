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
using VirtualDean.Enties;

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

        public BaseModel GetAuthenticatedBrother(BaseModel brother)
        {
            brother.JwtToken = _auth.GenerateToken(brother);
            return brother;
        }

        public async Task<IEnumerable<BaseModel>> GetBaseBrothersModel()
        {
            return await _brotherContext.Brothers.Where(bro => bro.StatusBrother == BrotherStatus.BRAT).Select(bro => new BaseModel() 
            { Id = bro.Id, Name = bro.Name, Surname = bro.Surname }).ToListAsync();
        }

        public async Task<Brother> GetBrother(int brotherId)
        {
            return await _brotherContext.Brothers.FindAsync(brotherId);
        }

        public async Task<IEnumerable<BaseBrotherForLiturgistOffice>> GetBrotherForLiturgistOffice()
        {
            return await _brotherContext.Brothers.Where(bro => !bro.IsDiacon).
                Select(bro => new BaseBrotherForLiturgistOffice { Id = bro.Id, Name = bro.Name, Surname = bro.Surname,
                StatusBrother = bro.StatusBrother, IsAcolit = bro.IsAcolit })
                .ToListAsync();
        }

        public async Task<IEnumerable<Brother>> GetBrothers()
        {
            return await _brotherContext.Brothers.ToListAsync();
        }

        public async Task<IEnumerable<BaseModel>> GetBrothersForCommunion()
        {
            return await _brotherContext.Brothers.Where(bro => (bro.IsAcolit || bro.IsDiacon) && bro.StatusBrother == "BRAT").ToListAsync();
        }

        public async Task<IEnumerable<BaseModel>> GetBrothersForTray()
        {
            return await _brotherContext.Brothers.Where(bro => !bro.IsAcolit && !bro.IsDiacon).ToListAsync();
        }

        public async Task<IEnumerable<CantorResponse>> GetSingingBrothers()
        {
            return await _brotherContext.Brothers.
                Where(bro => bro.IsSinging).
                Select(bro => new CantorResponse()
            { Id = bro.Id, Name = bro.Name, Surname = bro.Surname, IsSinging = bro.IsSinging}).ToListAsync();
        }

        public async Task<Boolean> IsBrotherInDb(Brother brother)
        {
            return await _brotherContext.Brothers.AnyAsync(bro => bro.Name == brother.Name && bro.Surname == brother.Surname);
        }

        public async Task SaveBrother(Brother brother)
        {
            try
            {
                brother.StatusBrother = BrotherStatus.BRAT;
                brother.PasswordHash = _auth.GetHashedPassword(brother.Name + "123");
                await _brotherContext.Brothers.AddAsync(brother);
                await _brotherContext.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task SetupBrothersTable()
        {
            if(! await _brotherContext.Brothers.AnyAsync())
            {
                await SetupAdminAccounts();
            }    
        }

        private async Task SetupAdminAccounts()
        {
            var cantor = new Brother
            {
                Name = "Kantor",
                Surname = "Studentatu",
                Email = "kantor.studentatu@gmail.com",
                StatusBrother = BrotherStatus.CANTOR,
                Precedency = DateTime.Now,
                IsSinging = false,
                IsLector = false,
                IsAcolit = false,
                IsDiacon = false,
                PasswordHash = _auth.GetHashedPassword("Kantor123")
            };

            var liturgist = new Brother
            {
                Name = "Liturgista",
                Surname = "Studentatu",
                Email = "liturgista.studentatu@gmail.com",
                StatusBrother = BrotherStatus.LITURGIST,
                Precedency = DateTime.Now,
                IsSinging = false,
                IsLector = false,
                IsAcolit = false,
                IsDiacon = false,
                PasswordHash = _auth.GetHashedPassword("Liturgista123")
            };

            var dean = new Brother
            {
                Name = "Dziekan",
                Surname = "Studentatu",
                Email = "dziekan.studentatu@gmail.com",
                StatusBrother = BrotherStatus.DEAN,
                Precedency = DateTime.Now,
                IsSinging = false,
                IsLector = false,
                IsAcolit = false,
                IsDiacon = false,
                PasswordHash = _auth.GetHashedPassword("Dziekan123")
            };

            var deanCommunion = new Brother
            {
                Name = "Dziekan",
                Surname = "Komunijny",
                Email = "dziekan.studentatu@gmail.com",
                StatusBrother = BrotherStatus.COMMUNION_DEAN,
                Precedency = DateTime.Now,
                IsSinging = false,
                IsLector = false,
                IsAcolit = false,
                IsDiacon = false,
                PasswordHash = _auth.GetHashedPassword("Komunijny23")
            };

            await _brotherContext.AddAsync(cantor);
            await _brotherContext.AddAsync(liturgist);
            await _brotherContext.AddAsync(dean);
            await _brotherContext.AddAsync(deanCommunion);

            await _brotherContext.SaveChangesAsync();
        }
    }
}
