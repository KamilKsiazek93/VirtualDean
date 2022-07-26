using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtualDean.Data;
using VirtualDean.Models;
using VirtualDean.Enties;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http;
using System.Net.Http.Json;

namespace VirtualDean.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class OfficesController : ControllerBase
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IOfficesManager _officesManager;
        
        public OfficesController(IHttpClientFactory clientFactory, IOfficesManager officesManager)
        {
            _clientFactory = clientFactory;
            _officesManager = officesManager;
        }

        [Authorize(Policy = "Cantor")]
        [HttpPost("singing")]
        public async Task<ActionResult> AddScholaOffices(IEnumerable<Office> offices)
        {
            try
            {
                if(!await IsOfficeAvailableToSet(PipelineConstName.CANTOR))
                {
                    return NotFound(new { message = ActionResultMessage.OfficeNotAdded });
                }
                await _officesManager.AddBrothersForSchola(offices);
                await _officesManager.UpdatePipelineStatus(PipelineConstName.CANTOR, false);
                await _officesManager.UpdatePipelineStatus(PipelineConstName.TRAY, true);
                return Ok(new { message = ActionResultMessage.OfficeAdded });
            }
            catch
            {
                return NotFound(new { message = ActionResultMessage.OfficeNotAdded });
            }
        }

        [HttpGet("office-name/{name}")]
        public async Task<IEnumerable<OfficeNames>> GetOfficeNames(string name)
        {
             return await _officesManager.GetOfficeNames(name);
        }

        [HttpGet("office/{weekId}")]
        public async Task<IEnumerable<Office>> GetOffices(int weekId)
        {
            return await _officesManager.GetOffice(weekId);
        }

        [HttpGet("office-flat/{weekId}")]
        public async Task<IEnumerable<FlatOffice>> GetFlatOffice(int weekId)
        {
            return await _officesManager.GetFlatOffice(weekId);
        }

        [HttpGet("office-name/obstacle")]
        public async Task<IEnumerable<string>> GetOfficeNameForObstacle()
        {
            var officeName = await _officesManager.GetOfficeNamesForObstacle();
            var client = _clientFactory.CreateClient("Trays");
            var officeHour = await client.GetFromJsonAsync<IEnumerable<string>>("hours");
            return officeName.Union(officeHour);
        }

        [Authorize(Policy = "Liturgist")]
        [HttpPost("liturgist")]
        public async Task<ActionResult> AddLiturgistOffice(IEnumerable<Office> offices)
        {
            try
            {
                if (!await IsOfficeAvailableToSet(PipelineConstName.LITURGIST))
                {
                    return NotFound(new { message = ActionResultMessage.OfficeNotAdded });
                }
                await _officesManager.AddLiturgistOffice(offices);
                await _officesManager.UpdatePipelineStatus(PipelineConstName.LITURGIST, false);
                await _officesManager.UpdatePipelineStatus(PipelineConstName.DEAN, true);
                return Ok(new { message = ActionResultMessage.OfficeAdded });
            }
            catch
            {
                return NotFound(new { message = ActionResultMessage.OfficeNotAdded });
            }
        }

        [Authorize(Policy = "Dean")]
        [HttpPost("dean")]
        public async Task<ActionResult> AddDeanOffice(IEnumerable<Office> offices)
        {
            try
            {
                var client = _clientFactory.CreateClient("Weeks");
                if (!await IsOfficeAvailableToSet(PipelineConstName.DEAN))
                {
                    return NotFound(new { message = ActionResultMessage.OfficeNotAdded });
                }
                await _officesManager.AddDeanOffice(offices);
                await client.GetFromJsonAsync<Task>("");
                await _officesManager.UpdatePipelineStatus(PipelineConstName.DEAN, false);
                await _officesManager.UpdatePipelineStatus(PipelineConstName.KITCHEN, true);
                return Ok(new { message = ActionResultMessage.OfficeAdded });
            }
            catch
            {
                return NotFound(new { message = ActionResultMessage.OfficeNotAdded });
            }
        }


        [HttpGet("office-last/{brotherId}")]
        public async Task<OfficeBrother> GetLastOfficeForBrother(int brotherId)
        {
            var clientTray = _clientFactory.CreateClient("Trays");
            var clientCommunion = _clientFactory.CreateClient("Communions");
            var clientWeek = _clientFactory.CreateClient("Weeks");
            int weekNumber = await clientWeek.GetFromJsonAsync<int>("") - 1;
            var trays = await clientTray.GetFromJsonAsync<IEnumerable<string>>($"brother/{brotherId}");
            var communions = await clientCommunion.GetFromJsonAsync<IEnumerable<string>>($"brother/{brotherId}");
            var otherOffices = await _officesManager.GetOfficeForBrother(weekNumber, brotherId);
            return _officesManager.GetOfficeForSingleBrother(trays, communions, otherOffices);
        }

        [HttpGet("office-previous/{brotherId}")]
        public async Task<OfficeBrother> GetPreviousOfficeForBrother(int brotherId)
        {
            var clientTray = _clientFactory.CreateClient("Trays");
            var clientCommunion = _clientFactory.CreateClient("Communions");
            var clientWeek = _clientFactory.CreateClient("Weeks");
            int weekNumber = await clientWeek.GetFromJsonAsync<int>("") - 2;
            var trays = await clientTray.GetFromJsonAsync<IEnumerable<string>>($"brother/{brotherId}");
            var communions = await clientCommunion.GetFromJsonAsync<IEnumerable<string>>($"brother/{brotherId}");
            var otherOffices = await _officesManager.GetOfficeForBrother(weekNumber, brotherId);
            return _officesManager.GetOfficeForSingleBrother(trays, communions, otherOffices);
        }

        [Authorize(Policy = "Dean")]
        [HttpGet("office-last")]
        public async Task<IEnumerable<OfficePrint>> GetLastOffice()
        {
            var clientWeek = _clientFactory.CreateClient("Weeks");
            var clientBrother = _clientFactory.CreateClient("Brothers");
            var clientTray = _clientFactory.CreateClient("Trays");
            var clientCommunion = _clientFactory.CreateClient("Communions");
            int weekNumber = await clientWeek.GetFromJsonAsync<int>("") - 1;
            var brothers = (await clientBrother.GetFromJsonAsync<IEnumerable<Brother>>("")).OrderBy(bro => bro.Precedency);
            var trays = await clientTray.GetFromJsonAsync<IEnumerable<TrayOfficeAdded>>($"week/{weekNumber}");
            var communions = await clientCommunion.GetFromJsonAsync<IEnumerable<CommunionOfficeAdded>>($"week/{weekNumber}");
            var otherOffices = await _officesManager.GetOffice(weekNumber);
            return _officesManager.GetOfficeForAllBrothers(brothers, trays, communions, otherOffices);
        }

        [Authorize(Policy = "Dean")]
        [HttpPost("kitchen")]
        public async Task<ActionResult> AddKitchenOffices(IEnumerable<KitchenOffices> offices)
        {
            try
            {
                if (!await IsOfficeAvailableToSet(PipelineConstName.KITCHEN))
                {
                    return NotFound(new { message = ActionResultMessage.OfficeNotAdded });
                }
                await _officesManager.AddKitchenOffices(offices);
                await _officesManager.UpdatePipelineStatus(PipelineConstName.KITCHEN, false);
                await _officesManager.UpdatePipelineStatus(PipelineConstName.CANTOR, true);
                await _officesManager.UpdatePipelineStatus(PipelineConstName.COMMUNION, true);
                return Ok(new { message = ActionResultMessage.OfficeAdded });
            }
            catch
            {
                return NotFound(new { message = ActionResultMessage.OfficeNotAdded });
            }
        }

        [HttpGet("kitchen")]
        public async Task<IEnumerable<KitchenOffices>> GetKitchenOffices()
        {
            return await _officesManager.GetKitchenOffices();
        }

        [HttpGet("kitchen/{weekId}")]
        public async Task<IEnumerable<KitchenOffices>> GetKitchenOffices(int weekId)
        {
            return await _officesManager.GetKitchenOffices(weekId);
        }

        [HttpGet("names")]
        public async Task<IEnumerable<string>> GetOfficesName()
        {
            return await _officesManager.GetOfficesName();
        }

        [HttpGet("pipeline/name/{name}")]
        public async Task<Boolean> IsOfficeAvailableToSet(string name)
        {
            return await _officesManager.GetPipelineStatus(name);
        }

        [HttpPut("pipeline")]
        public async Task UpdatePipelineStatus(PipelineUpdate pipelineUpdate)
        {
            await _officesManager.UpdatePipelineStatus(pipelineUpdate.JobName, pipelineUpdate.JobValue);
        }
    }
}
