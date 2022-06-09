using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using VirtualDean.Models;
using VirtualDean.Enties;
using VirtualDean.Models.DatabaseContext;

namespace VirtualDean.Data
{
    public class OfficesManager : IOfficesManager
    {
        private readonly OfficeNameDbContext _officeNameContext;
        private readonly OfficeDbContext _officeDbContext;
        private readonly KitchenOfficeDbContext _kitchenContext;
        private readonly PipelineStatusDbContext _pipelineContext;
        private readonly IWeek _week;

        public OfficesManager(IWeek week, OfficeNameDbContext officeNameDbContext,
            OfficeDbContext officeDbContext, KitchenOfficeDbContext kitchenContext, PipelineStatusDbContext pipelineContext)
        {
            _officeNameContext = officeNameDbContext;
            _week = week;
            _officeDbContext = officeDbContext;
            _kitchenContext = kitchenContext;
            _pipelineContext = pipelineContext;
        }

        public async Task AddKitchenOffices(IEnumerable<KitchenOffices> kitchenOffices)
        {
            int weekOfOffice = await _week.GetLastWeek();
            foreach(var office in kitchenOffices)
            {
                office.WeekOfOffices = weekOfOffice;
            }

            await _kitchenContext.AddRangeAsync(kitchenOffices);
            await _kitchenContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<KitchenOffices>> GetKitchenOffices()
        {
            return await _kitchenContext.KitchenOffice.ToListAsync();
        }

        public async Task<IEnumerable<KitchenOffices>> GetKitchenOffices(int weekId)
        {
            return await _kitchenContext.KitchenOffice.Where(office => office.WeekOfOffices == weekId).ToListAsync();
        }

        public async Task AddBrothersForSchola(IEnumerable<Office> offices)
        {
            int weekNumber = await _week.GetLastWeek();
            foreach(var office in offices)
            {
                office.WeekOfOffices = weekNumber;
                
                await _officeDbContext.AddAsync(office);
            }
            await _officeDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<string>> GetOfficesName()
        {
            return await _officeNameContext.OfficeNames.Select(i => i.OfficeName).ToListAsync();
        }

        public async Task<IEnumerable<Office>> GetLastOffice()
        {
            int weekOfOffice = await _week.GetLastWeek();
            return await _officeDbContext.Offices.Where(item => item.WeekOfOffices == weekOfOffice).ToListAsync();
        }

        public async Task<Office> GetOfficeForBrother(int weekNumber, int brotherId)
        {
            var offices = await _officeDbContext.Offices.Where(item => item.BrotherId == brotherId && item.WeekOfOffices == weekNumber).ToListAsync();
            return new Office
            {
                CantorOffice = offices.Where(item => item.CantorOffice != null).Select(item => item.CantorOffice).FirstOrDefault(),
                LiturgistOffice = offices.Where(item => item.LiturgistOffice != null).Select(item => item.LiturgistOffice).FirstOrDefault(),
                DeanOffice = offices.Where(item => item.DeanOffice != null).Select(item => item.DeanOffice).FirstOrDefault()
            };
        }

        public async Task AddLiturgistOffice(IEnumerable<Office> offices)
        {
            int weekNumber = await _week.GetLastWeek();
            foreach (var office in offices)
            {
                office.WeekOfOffices = weekNumber;

                await _officeDbContext.AddAsync(office);
            }
            await _officeDbContext.SaveChangesAsync();
        }

        public async Task AddDeanOffice(IEnumerable<Office> offices)
        {
            int weekNumber = await _week.GetLastWeek();
            foreach (var office in offices)
            {
                office.WeekOfOffices = weekNumber;

                await _officeDbContext.AddAsync(office);
            }
            await _officeDbContext.SaveChangesAsync();
        }

        public async Task<bool> IsScholaAlreadySet()
        {
            int weekNumber = await _week.GetLastWeek();
            return await _officeDbContext.Offices.Where(item => item.WeekOfOffices == weekNumber && item.CantorOffice != null)
                .Select(item => item.CantorOffice).AnyAsync();
        }

        public async Task<bool> IsKitchenOfficeAlreadySet()
        {
            int weekNumber = await _week.GetLastWeek();
            return await _kitchenContext.KitchenOffice.Where(item => item.WeekOfOffices == weekNumber).AnyAsync();
        }

        public async Task<bool> IsLiturgistOfficeAlreadySet()
        {
            int weekNumber = await _week.GetLastWeek();
            return await _officeDbContext.Offices.Where(item => item.WeekOfOffices == weekNumber && item.LiturgistOffice != null)
                .Select(item => item.LiturgistOffice).AnyAsync();
        }

        public async Task<bool> IsDeanOfficeAlreadySet(int weekNumber)
        {
            return await _officeDbContext.Offices.Where(item => item.WeekOfOffices == weekNumber && item.DeanOffice != null)
                .Select(item => item.DeanOffice).AnyAsync();
        }

        public async Task<bool> GetPipelineStatus(string name)
        {
            return await _pipelineContext.PipelineStatus.Where(item => item.Name == name).Select(item => item.PipelineValue).FirstOrDefaultAsync();
        }

        public async Task UpdatePipelineStatus(string jobName, Boolean jobValue)
        {
            var finishedJob = await _pipelineContext.PipelineStatus.Where(item => item.Name == jobName).FirstOrDefaultAsync();
            finishedJob.PipelineValue = jobValue;
            _pipelineContext.Entry(finishedJob).State = EntityState.Modified;
            await _pipelineContext.SaveChangesAsync();
        }
    }
}
