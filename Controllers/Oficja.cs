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
        [HttpGet("brothers")]
        public IEnumerable<Brother> GetBrothers()
        {
            List<Brother> brothers = new List<Brother>();
            return brothers;
        }

        [HttpPost("brothers")]
        public void AddBrothers(Brother brother)
        {
            //
        }

        [HttpPost("kitchen-offices")]
        public void AddKitchenOffices(IEnumerable<KitchenOffices> offices)
        {
            //
        }

        [HttpGet("kitchen-offices")]
        public IEnumerable<KitchenOffices> GetKitchenOffices()
        {
            List<KitchenOffices> kitchen = new List<KitchenOffices>();
            return kitchen;
        }

        [HttpGet("kitchen-offices/{weekId}")]
        public IEnumerable<KitchenOffices> GetKitchenOffices(int weekId)
        {
            List<KitchenOffices> kitchen = new List<KitchenOffices>();
            return kitchen;
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
