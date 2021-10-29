﻿using Microsoft.AspNetCore.Http;
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
        private readonly IKitchen _kitchen;
        private readonly ITrayHour _trayHour;
        private readonly ICommunionHour _communionHour;
        public Offices(IBrothers brothers, IKitchen kitchen, ITrayHour trayHour, ICommunionHour communionHour)
        {
            _brothers = brothers;
            _kitchen = kitchen;
            _trayHour = trayHour;
            _communionHour = communionHour;
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
        public async Task AddTrayOffice(IEnumerable<TrayOfficeAdded> listOfTray)
        {
            await _trayHour.AddTrayHour(listOfTray);
        }

        [HttpGet("tray-hour")]
        public async Task<IEnumerable<TrayOfficeAdded>> GetTrayHour()
        {
            return await _trayHour.GetTrayHours();
        }

        [HttpGet("tray-hour/{weekId}")]
        public async Task<IEnumerable<TrayOfficeAdded>> GetTrayHour(int weekId)
        {
            return await _trayHour.GetTrayHours(weekId);
        }

        [HttpPost("communion-hour")]
        public async Task AddCommunionOffice(IEnumerable<CommunionOfficeAdded> listOfCommunion)
        {
            await _communionHour.AddCommunionHour(listOfCommunion);
        }

        [HttpGet("communion-hour")]
        public async Task<IEnumerable<CommunionOfficeAdded>> GetCommunionHour()
        {
            return await _communionHour.GetCommunionHours();
        }

        [HttpGet("communion-hour/{weekId}")]
        public async Task<IEnumerable<CommunionOfficeAdded>> GetCommunionHour(int weekId)
        {
            return await _communionHour.GetCommunionHours(weekId);
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