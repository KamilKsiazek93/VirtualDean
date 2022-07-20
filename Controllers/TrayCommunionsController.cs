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
    public class TrayCommunionsController : ControllerBase
    {
        private readonly ITrayCommunionHour _trayCommunionHour;
        private readonly IHttpClientFactory _clientFactory;

        public TrayCommunionsController(ITrayCommunionHour trayCommunionHour, IHttpClientFactory clientFactory)
        {
            _trayCommunionHour = trayCommunionHour;
            _clientFactory = clientFactory;
        }

        [HttpPost("tray-hour")]
        public async Task<ActionResult> AddTrayOffice(IEnumerable<TrayOfficeAdded> listOfTray)
        {
            try
            {
                var client = _clientFactory.CreateClient("Offices");
                if (!await IsOfficeAvailableToSet(PipelineConstName.TRAY))
                {
                    return NotFound(new { message = ActionResultMessage.OfficeNotAdded });
                }
               // if (IsCurrenUserLiturgist())
                //{
                    await _trayCommunionHour.AddTrayHour(listOfTray);
                    await client.PutAsJsonAsync("pipeline", new PipelineUpdate { JobName = PipelineConstName.TRAY, JobValue = false});
                    await client.PutAsJsonAsync("pipeline", new PipelineUpdate { JobName = PipelineConstName.LITURGIST, JobValue = true});
                    return Ok(new { message = ActionResultMessage.OfficeAdded });
               // }
               // return Unauthorized(new { message = ActionResultMessage.UnauthorizedUser });
            }
            catch
            {
                return NotFound(new { message = ActionResultMessage.OperationFailed });
            }
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

        [HttpGet("tray-hour-last")]
        public async Task<IEnumerable<LastTrayOfficeList>> GetLastTray()
        {
            return await _trayCommunionHour.GetLastTrayHour();
        }

        [HttpGet("tray-hour-last/{brotherId}")]
        public async Task<IEnumerable<string>> GetLastTrayForBrother(int brotherId)
        {
            var client = _clientFactory.CreateClient("Weeks");
            int weekNumber = await client.GetFromJsonAsync<int>("");
            return await _trayCommunionHour.GetTrayHour(weekNumber, brotherId);
        }

        [HttpPost("communion-hour")]
        public async Task<ActionResult> AddCommunionOffice(IEnumerable<CommunionOfficeAdded> listOfCommunion)
        {
            if (!await IsOfficeAvailableToSet(PipelineConstName.COMMUNION))
            {
                return NotFound(new { message = ActionResultMessage.OfficeNotAdded });
            }
            try
            {
                var client = _clientFactory.CreateClient("Offices");
                await _trayCommunionHour.AddCommunionHour(listOfCommunion);
                await client.PutAsJsonAsync("pipeline", new PipelineUpdate { JobName = PipelineConstName.COMMUNION, JobValue = false });
                return Ok(new { message = ActionResultMessage.OfficeAdded });
            }
            catch
            {
                return NotFound(new { message = ActionResultMessage.OperationFailed });
            }
        }

        private Task<bool> IsOfficeAvailableToSet(string officeName)
        {
            var client = _clientFactory.CreateClient("Offices");
            return client.GetFromJsonAsync<bool>($"pipeline-status/{officeName}");
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

        [HttpGet("communion-hour-last/{brotherId}")]
        public async Task<IEnumerable<string>> GetLastCommunionForBrother(int brotherId)
        {
            var client = _clientFactory.CreateClient("Weeks");
            int weekNumber = await client.GetFromJsonAsync<int>("");
            return await _trayCommunionHour.GetCommunionHour(weekNumber, brotherId);
        }

        [HttpGet("hours-tray")]
        public Task<IEnumerable<string>> GetHoursForTray()
        {
            return _trayCommunionHour.GetHoursForTray();
        }

        [HttpGet("hours-communion")]
        public Task<IEnumerable<string>> GetHoursForCommunion()
        {
            return _trayCommunionHour.GetHoursForCommunion();
        }
    }
}
