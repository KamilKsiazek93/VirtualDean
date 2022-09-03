using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtualDean.Models;
using VirtualDean.Models.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using VirtualDean.Enties;

namespace VirtualDean.Data
{
    public class Brothers : IBrothers
    {
        private readonly VirtualDeanDbContext _virtualDeanDbContext;
        private readonly IAuth _auth;
        public Brothers(VirtualDeanDbContext virtualDeanDbContext, IAuth auth)
        {
            _virtualDeanDbContext = virtualDeanDbContext;
            _auth = auth;
        }

        public async Task DeleteBrother(Brother brother)
        {
            _virtualDeanDbContext.Remove(brother);
            await _virtualDeanDbContext.SaveChangesAsync();
        }

        public async Task EditBrother(Brother brother)
        {
            _virtualDeanDbContext.Entry(brother).State = EntityState.Modified;
            await _virtualDeanDbContext.SaveChangesAsync();
        }

        public async Task<BaseModel> FindLoginBrother(LoginModel loginData)
        {
            var hashedPassword = _auth.GetHashedPassword(loginData.Password);
            var brother = await _virtualDeanDbContext.Brothers.Where(b => b.Email == loginData.Email 
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
            return await _virtualDeanDbContext.Brothers
                .OrderBy(bro => bro.Precedency)
                .Where(bro => bro.StatusBrother == BrotherStatus.BRAT).Select(bro => new BaseModel() 
                { Id = bro.Id, Name = bro.Name, Surname = bro.Surname }).ToListAsync();
        }

        public async Task<Brother> GetBrother(int brotherId)
        {
            return await _virtualDeanDbContext.Brothers.FindAsync(brotherId);
        }

        public async Task<IEnumerable<BaseBrotherForLiturgistOffice>> GetBrotherForLiturgistOffice()
        {
            return await _virtualDeanDbContext.Brothers.OrderBy(bro => bro.Precedency).Where(bro => !bro.IsDiacon && bro.StatusBrother == BrotherStatus.BRAT).
                Select(bro => new BaseBrotherForLiturgistOffice { Id = bro.Id, Name = bro.Name, Surname = bro.Surname,
                StatusBrother = bro.StatusBrother, IsAcolit = bro.IsAcolit, IsLector = bro.IsLector })
                .ToListAsync();
        }

        public async Task<IEnumerable<Brother>> GetBrothers()
        {
            return await _virtualDeanDbContext.Brothers.OrderBy(bro => bro.Precedency).Where(bro => bro.StatusBrother == BrotherStatus.BRAT).ToListAsync();
        }

        public async Task<IEnumerable<BaseModel>> GetBrothersForCommunion()
        {
            return await _virtualDeanDbContext.Brothers
                .OrderBy(bro => bro.Precedency)
                .Where(bro => (bro.IsAcolit || bro.IsDiacon) && bro.StatusBrother == BrotherStatus.BRAT).ToListAsync();
        }

        public async Task<IEnumerable<BaseModel>> GetBrothersForTray()
        {
            return await _virtualDeanDbContext.Brothers.Where(bro => !bro.IsAcolit && !bro.IsDiacon && bro.StatusBrother == BrotherStatus.BRAT).ToListAsync();
        }

        public string GetHashedPassword(string currentPassword)
        {
            return _auth.GetHashedPassword(currentPassword);
        }

        public async Task<IEnumerable<CantorResponse>> GetSingingBrothers()
        {
            return await _virtualDeanDbContext.Brothers
                .OrderBy(bro => bro.Precedency)
                .Where(bro => bro.IsSinging && !bro.IsDiacon && bro.StatusBrother == BrotherStatus.BRAT).
                Select(bro => new CantorResponse()
            { Id = bro.Id, Name = bro.Name, Surname = bro.Surname, IsSinging = bro.IsSinging}).ToListAsync();
        }

        public async Task<Boolean> IsBrotherInDb(Brother brother)
        {
            return await _virtualDeanDbContext.Brothers.AnyAsync(bro => bro.Name == brother.Name && bro.Surname == brother.Surname);
        }

        public async Task SaveBrother(Brother brother)
        {
            try
            {
                brother.StatusBrother = BrotherStatus.BRAT;
                brother.PasswordHash = _auth.GetHashedPassword(brother.Name + "123");
                await _virtualDeanDbContext.Brothers.AddAsync(brother);
                await _virtualDeanDbContext.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task SetupBrothersTable()
        {
            if(! await _virtualDeanDbContext.Brothers.AnyAsync())
            {
                await SetupAdminAccounts();
            }    
        }

        public async Task UpdatePassword(PasswordUpdate passwordUpdate, Brother brother)
        {
            if(brother.PasswordHash != _auth.GetHashedPassword(passwordUpdate.CurrentPassword))
            {
                throw new Exception("Podane hasło nie jest zgodne z obecnym hasłem");
            }
            else
            {
                brother.PasswordHash = _auth.GetHashedPassword(passwordUpdate.NewPassword);
                _virtualDeanDbContext.Entry(brother).State = EntityState.Modified;
                await _virtualDeanDbContext.SaveChangesAsync();
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
                Email = "dziekan.komunijny@gmail.com",
                StatusBrother = BrotherStatus.COMMUNION_DEAN,
                Precedency = DateTime.Now,
                IsSinging = false,
                IsLector = false,
                IsAcolit = false,
                IsDiacon = false,
                PasswordHash = _auth.GetHashedPassword("Komunijny123")
            };

            await _virtualDeanDbContext.AddAsync(cantor);
            await _virtualDeanDbContext.AddAsync(liturgist);
            await _virtualDeanDbContext.AddAsync(dean);
            await _virtualDeanDbContext.AddAsync(deanCommunion);

            await _virtualDeanDbContext.SaveChangesAsync();
        }
    }
}
