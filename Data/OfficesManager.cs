using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtualDean.Models;
using VirtualDean.Enties;
using VirtualDean.Models.DatabaseContext;

namespace VirtualDean.Data
{
    public class OfficesManager : IOfficesManager
    {
        private readonly VirtualDeanDbContext _virtualDeanDbContext;
        private readonly IWeek _week;

        public OfficesManager(IWeek week, VirtualDeanDbContext virtualDeanDbContext)
        {
            _virtualDeanDbContext = virtualDeanDbContext;
            _week = week;
        }

        public async Task AddKitchenOffices(IEnumerable<KitchenOffices> kitchenOffices)
        {
            int weekOfOffice = await _week.GetLastWeek();
            foreach(var office in kitchenOffices)
            {
                office.WeekOfOffices = weekOfOffice;
            }

            await _virtualDeanDbContext.AddRangeAsync(kitchenOffices);
            await _virtualDeanDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<KitchenOffices>> GetKitchenOffices()
        {
            return await _virtualDeanDbContext.KitchenOffice.ToListAsync();
        }

        public async Task<IEnumerable<KitchenOffices>> GetKitchenOffices(int weekId)
        {
            return await _virtualDeanDbContext.KitchenOffice.Where(office => office.WeekOfOffices == weekId).ToListAsync();
        }

        public async Task AddBrothersForSchola(IEnumerable<Office> offices)
        {
            int weekNumber = await _week.GetLastWeek();
            foreach(var office in offices)
            {
                office.WeekOfOffices = weekNumber;
                
                await _virtualDeanDbContext.AddAsync(office);
            }
            await _virtualDeanDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<string>> GetOfficesName()
        {
            return await _virtualDeanDbContext.OfficeNames.Select(i => i.OfficeName).ToListAsync();
        }

        public async Task<IEnumerable<Office>> GetLastOffice()
        {
            int weekOfOffice = await _week.GetLastWeek();
            return await _virtualDeanDbContext.Offices.Where(item => item.WeekOfOffices == weekOfOffice).ToListAsync();
        }

        public async Task<IEnumerable<Office>> GetOffice(int weekId)
        {
            return await _virtualDeanDbContext.Offices.Where(item => item.WeekOfOffices == weekId).ToListAsync();
        }

        public async Task<IEnumerable<FlatOffice>> GetFlatOffice(int weekId)
        {
            var offices = await GetOffice(weekId);
            var flatOffices = new List<FlatOffice>();
            foreach(var office in offices)
            {
                flatOffices.Add(new FlatOffice { BrotherId = office.BrotherId, OfficeName = GetNotNullPropertyFromOffice(office) });
            }
            return flatOffices;
        }

        private string GetNotNullPropertyFromOffice(Office office)
        {
            if(office.CantorOffice != null)
            {
                return office.CantorOffice;
            }
            else if(office.LiturgistOffice != null)
            {
                return office.LiturgistOffice;
            }
            return office.DeanOffice;
        }

        public async Task<Office> GetOfficeForBrother(int weekNumber, int brotherId)
        {
            var offices = await _virtualDeanDbContext.Offices.Where(item => item.BrotherId == brotherId && item.WeekOfOffices == weekNumber).ToListAsync();
            return new Office
            {
                CantorOffice = offices.Where(item => item.CantorOffice != null).Select(item => item.CantorOffice).FirstOrDefault(),
                LiturgistOffice = offices.Where(item => item.LiturgistOffice != null).Select(item => item.LiturgistOffice).FirstOrDefault(),
                DeanOffice = offices.Where(item => item.DeanOffice != null).Select(item => item.DeanOffice).FirstOrDefault(),
                BrotherId = brotherId
            };
        }

        public async Task AddLiturgistOffice(IEnumerable<Office> offices)
        {
            int weekNumber = await _week.GetLastWeek();
            foreach (var office in offices)
            {
                office.WeekOfOffices = weekNumber;

                await _virtualDeanDbContext.AddAsync(office);
            }
            await _virtualDeanDbContext.SaveChangesAsync();
        }

        public async Task AddDeanOffice(IEnumerable<Office> offices)
        {
            int weekNumber = await _week.GetLastWeek();
            foreach (var office in offices)
            {
                office.WeekOfOffices = weekNumber;

                await _virtualDeanDbContext.AddAsync(office);
            }
            await _virtualDeanDbContext.SaveChangesAsync();
        }

        public async Task<bool> IsScholaAlreadySet()
        {
            int weekNumber = await _week.GetLastWeek();
            return await _virtualDeanDbContext.Offices.Where(item => item.WeekOfOffices == weekNumber && item.CantorOffice != null)
                .Select(item => item.CantorOffice).AnyAsync();
        }

        public async Task<bool> IsKitchenOfficeAlreadySet()
        {
            int weekNumber = await _week.GetLastWeek();
            return await _virtualDeanDbContext.KitchenOffice.Where(item => item.WeekOfOffices == weekNumber).AnyAsync();
        }

        public async Task<bool> IsLiturgistOfficeAlreadySet()
        {
            int weekNumber = await _week.GetLastWeek();
            return await _virtualDeanDbContext.Offices.Where(item => item.WeekOfOffices == weekNumber && item.LiturgistOffice != null)
                .Select(item => item.LiturgistOffice).AnyAsync();
        }

        public async Task<bool> IsDeanOfficeAlreadySet(int weekNumber)
        {
            return await _virtualDeanDbContext.Offices.Where(item => item.WeekOfOffices == weekNumber && item.DeanOffice != null)
                .Select(item => item.DeanOffice).AnyAsync();
        }

        public async Task<bool> GetPipelineStatus(string name)
        {
            return await _virtualDeanDbContext.PipelineStatus.Where(item => item.Name == name).Select(item => item.PipelineValue).FirstOrDefaultAsync();
        }

        public async Task UpdatePipelineStatus(string jobName, Boolean jobValue)
        {
            var finishedJob = await _virtualDeanDbContext.PipelineStatus.Where(item => item.Name == jobName).FirstOrDefaultAsync();
            finishedJob.PipelineValue = jobValue;
            _virtualDeanDbContext.Entry(finishedJob).State = EntityState.Modified;
            await _virtualDeanDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<OfficeNames>> GetOfficeNames(string adminName)
        {
            return await _virtualDeanDbContext.OfficeNames.Where(item => item.OfficeAdmin == adminName).ToListAsync();
        }

        public OfficeBrother GetOfficeForSingleBrother(IEnumerable<string> trays, IEnumerable<string> communions, Office otherOffices)
        {
            return new OfficeBrother
            {
                BrotherId = otherOffices.BrotherId,
                CantorOffice = otherOffices.CantorOffice,
                Tray = trays,
                Communion = communions,
                LiturgistOffice = otherOffices.LiturgistOffice,
                DeanOffice = otherOffices.DeanOffice
            };
        }

        public OfficePrint GetOfficeForSingleBrotherPrint(IEnumerable<string> trays, IEnumerable<string> communions, Office otherOffices, BaseModel brother)
        {
            return new OfficePrint
            {
                BrotherId = brother.Id,
                Name = brother.Name,
                Surname = brother.Surname,
                CantorOffice = otherOffices.CantorOffice,
                Tray = trays,
                Communion = communions,
                LiturgistOffice = otherOffices.LiturgistOffice,
                DeanOffice = otherOffices.DeanOffice
            };
        }

        public async Task<IEnumerable<string>> GetOfficeNamesForObstacle()
        {
            return await _virtualDeanDbContext.OfficeNames.Where(item => item.OfficeAdmin != PipelineConstName.KITCHEN)
                .Select(item => item.OfficeName).ToListAsync();
        }

        public IEnumerable<OfficePrint> GetOfficeForAllBrothers(IEnumerable<Brother> brothers, IEnumerable<TrayOfficeAdded> trays, IEnumerable<CommunionOfficeAdded> communions, IEnumerable<Office> otherOffices)
        {
            var offices = new List<OfficePrint>();

            foreach(var brother in brothers)
            {
                offices.Add(new OfficePrint
                {
                    BrotherId = brother.Id,
                    Name = brother.Name,
                    Surname = brother.Surname,
                    Tray = trays.Where(item => item.BrotherId == brother.Id).Select(item => item.TrayHour).ToList(),
                    Communion = communions.Where(item => item.BrotherId == brother.Id).Select(item => item.CommunionHour).ToList(),
                    CantorOffice = otherOffices.Where(item => item.CantorOffice != null && item.BrotherId == brother.Id).Select(item => item.CantorOffice).FirstOrDefault(),
                    LiturgistOffice = otherOffices.Where(item => item.LiturgistOffice != null && item.BrotherId == brother.Id).Select(item => item.LiturgistOffice).FirstOrDefault(),
                    DeanOffice = otherOffices.Where(item => item.DeanOffice != null  && item.BrotherId == brother.Id).Select(item => item.DeanOffice).FirstOrDefault(),
                });
            }

            return offices;
        }
    }
}
