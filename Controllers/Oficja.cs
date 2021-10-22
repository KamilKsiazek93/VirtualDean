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
    public class Oficja : ControllerBase
    {
        private readonly IBrothers _brothers;
        private readonly IKitchen _kitchen;
        public Oficja(IBrothers brothers, IKitchen kitchen)
        {
            _brothers = brothers;
            _kitchen = kitchen;
        }
        [HttpGet("brothers")]
        public async Task<IEnumerable<Brother>> GetBrothers()
        {
            return await _brothers.GetBrothers();
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
            await _kitchen.AddKitchenOffices(offices);
        }

        [HttpGet("kitchen-offices")]
        public async Task<IEnumerable<KitchenOffices>> GetKitchenOffices()
        {
            return await _kitchen.GetKitchenOffices();
        }

        [HttpGet("kitchen-offices/{weekId}")]
        public async Task<IEnumerable<KitchenOffices>> GetKitchenOffices(int weekId)
        {
            return await _kitchen.GetKitchenOffices(weekId);
        }

        [HttpPost("tray-hour")]
        public IEnumerable<TrayOfficeAdded> AddTrayOffice(IEnumerable<TrayOfficeAdded> listOfTray)
        {
            List<TrayOfficeAdded> offices = new List<TrayOfficeAdded>();
            return offices;
        }

        [HttpPost("communion-hour")]
        public IEnumerable<CommunionOfficeAdded> AddCommunionOffice(IEnumerable<CommunionOfficeAdded> listOfCommunion)
        {
            List<CommunionOfficeAdded> offices = new List<CommunionOfficeAdded>();
            return offices;
        }

        [HttpPost("obstacles")]
        public void AddObstacles(IEnumerable<ObstaclesAdded> obstacles)
        {
            //
        }

        [HttpGet("obstacles")]
        public IEnumerable<ObstaclesDb> GetObstacles()
        {
            List<ObstaclesDb> obstacles = new List<ObstaclesDb>();
            return obstacles;
        }

        [HttpGet("obstacles/{weekId}")]
        public IEnumerable<ObstaclesDb> GetObstaclesInWeek(int weekId)
        {
            List<ObstaclesDb> obstacles = new List<ObstaclesDb>();
            return obstacles;
        }
    }
}
