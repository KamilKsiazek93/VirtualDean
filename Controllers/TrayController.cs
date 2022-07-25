using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using VirtualDean.Data;
using VirtualDean.Enties;
using VirtualDean.Models;

namespace VirtualDean.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrayController : ControllerBase
    {
        private readonly ITrayRepository _trayRepository;
        private readonly IHttpClientFactory _clientFactory;

        public TrayController(ITrayRepository trayRepository, IHttpClientFactory clientFactory)
        {
            _trayRepository = trayRepository;
            _clientFactory = clientFactory;
        }

        [Authorize(Policy = "Liturgist")]
        [HttpPost]
        public async Task<ActionResult> AddTrayOffice(IEnumerable<TrayOfficeAdded> listOfTray)
        {
            try
            {
                var client = _clientFactory.CreateClient("Offices");
                if (!await IsOfficeAvailableToSet(PipelineConstName.TRAY))
                {
                    return NotFound(new { message = ActionResultMessage.OfficeNotAdded });
                }
                await _trayRepository.AddTrayHour(listOfTray);
                await client.PutAsJsonAsync("pipeline", new PipelineUpdate { JobName = PipelineConstName.TRAY, JobValue = false });
                await client.PutAsJsonAsync("pipeline", new PipelineUpdate { JobName = PipelineConstName.LITURGIST, JobValue = true });
                return Ok(new { message = ActionResultMessage.OfficeAdded });
            }
            catch
            {
                return NotFound(new { message = ActionResultMessage.OperationFailed });
            }
        }

        [HttpGet]
        public async Task<IEnumerable<TrayOfficeAdded>> GetTrayHour()
        {
            return await _trayRepository.GetTrayHours();
        }

        [HttpGet("week/{weekId}")]
        public async Task<IEnumerable<TrayOfficeAdded>> GetTrayHour(int weekId)
        {
            return await _trayRepository.GetTrayHours(weekId);
        }

        [HttpGet("last")]
        public async Task<IEnumerable<LastTrayOfficeList>> GetLastTray()
        {
            return await _trayRepository.GetLastTrayHour();
        }

        [HttpGet("brother/{brotherId}")]
        public async Task<IEnumerable<string>> GetLastTrayForBrother(int brotherId)
        {
            var client = _clientFactory.CreateClient("Weeks");
            int weekNumber = await client.GetFromJsonAsync<int>("");
            return await _trayRepository.GetTrayHour(weekNumber, brotherId);
        }

        [HttpGet("hours")]
        public Task<IEnumerable<string>> GetHoursForTray()
        {
            return _trayRepository.GetHoursForTray();
        }

        private Task<bool> IsOfficeAvailableToSet(string officeName)
        {
            var client = _clientFactory.CreateClient("Offices");
            return client.GetFromJsonAsync<bool>($"pipeline-status/{officeName}");
        }
    }
}
