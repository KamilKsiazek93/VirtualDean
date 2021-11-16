using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtualDean.Data;
using VirtualDean.Models;

namespace VirtualDean.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Offices : ControllerBase
    {
        private readonly IBrothers _brothers;
        private readonly IOfficesManager _officesManager;
        private readonly ITrayCommunionHour _trayCommunionHour;
        private readonly IObstacle _obstacle;
        public Offices(IBrothers brothers, IOfficesManager officesManager, ITrayCommunionHour trayCommunionHour, IObstacle obstacle)
        {
            _brothers = brothers;
            _officesManager = officesManager;
            _trayCommunionHour = trayCommunionHour;
            _obstacle = obstacle;
        }

        [HttpGet("brothers")]
        public async Task<IEnumerable<Brother>> GetBrothers()
        {
            return await _brothers.GetBrothers();
        }

        [HttpGet("brothers-base")]
        public async Task<IEnumerable<BaseModel>> GetBaseBrothersModel()
        {
            return await _brothers.GetBaseBrothersModel();
        }

        [HttpGet("brothers/{id}")]
        public async Task<Brother> GetBrothers(int id)
        {
            return await _brothers.GetBrother(id);
        }

        [HttpPost("brothers")]
        public async Task<ActionResult<Brother>> AddBrothers(Brother brother)
        {
            var savedBrother = await _brothers.AddBrother(brother);
            return CreatedAtAction(nameof(GetBrothers), new { savedBrother });
        }

        [HttpPost("kitchen-offices")]
        public async Task AddKitchenOffices(IEnumerable<KitchenOffices> offices)
        {
            await _officesManager.AddKitchenOffices(offices);
        }

        [HttpGet("kitchen-offices")]
        public async Task<IEnumerable<KitchenOffices>> GetKitchenOffices()
        {
            return await _officesManager.GetKitchenOffices();
        }

        [HttpGet("kitchen-offices/{weekId}")]
        public async Task<IEnumerable<KitchenOffices>> GetKitchenOffices(int weekId)
        {
            return await _officesManager.GetKitchenOffices(weekId);
        }

        [HttpPost("tray-hour")]
        public async Task AddTrayOffice(IEnumerable<TrayOfficeAdded> listOfTray)
        {
            await _trayCommunionHour.AddTrayHour(listOfTray);
        }

        [HttpGet("tray-hour")]
        public async Task<IEnumerable<TrayOfficeAdded>> GetTrayHour()
        {
            return await _trayCommunionHour.GetTrayHours();
        }

        [HttpGet("tray-hour/{weekId}")]
        public async Task<IEnumerable<TrayOfficeAdded>> GetTrayHour(int weekId)
        {
            return await _trayCommunionHour.GetTrayHours(weekId);
        }

        [HttpPost("communion-hour")]
        public async Task AddCommunionOffice(IEnumerable<CommunionOfficeAdded> listOfCommunion)
        {
            await _trayCommunionHour.AddCommunionHour(listOfCommunion);
        }

        [HttpGet("communion-hour")]
        public async Task<IEnumerable<CommunionOfficeAdded>> GetCommunionHour()
        {
            return await _trayCommunionHour.GetCommunionHours();
        }

        [HttpGet("communion-hour/{weekId}")]
        public async Task<IEnumerable<CommunionOfficeAdded>> GetCommunionHour(int weekId)
        {
            return await _trayCommunionHour.GetCommunionHours(weekId);
        }

        [HttpPost("obstacles")]
        public async Task AddObstacles(IEnumerable<ObstaclesAdded> obstacles)
        {
            await _obstacle.AddObstacle(obstacles);
        }

        [HttpGet("obstacles/{weekId}")]
        public async Task<IEnumerable<ObstaclesAdded>> GetObstaclesInWeek(int weekId)
        {
            return await _obstacle.GetObstacles(weekId);
        }

        [HttpPost("obstacle-const")]
        public async Task AddConstObstacle(ConstObstacleAdded obstacles)
        {
            await _obstacle.AddConstObstacle(obstacles);
        }

        [HttpGet("obstacle-const/{brotherId}")]
        public async Task<IEnumerable<string>> GetConstObstacle(int brotherId)
        {
            return await _obstacle.GetConstObstacle(brotherId);
        }
    }
}
