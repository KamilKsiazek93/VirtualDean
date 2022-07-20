using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtualDean.Data;
using VirtualDean.Models;
using VirtualDean.Enties;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Net.Http;
using System.Net.Http.Json;

namespace VirtualDean.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfficesController : ControllerBase
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IOfficesManager _officesManager;
        
        public OfficesController(IHttpClientFactory clientFactory, IOfficesManager officesManager,
            ITrayCommunionHour trayCommunionHour, IObstacle obstacle, IWeek week)
        {
            _clientFactory = clientFactory;
            _officesManager = officesManager;
        }

        [HttpPost("office-singing")]
        public async Task<ActionResult> AddScholaOffices(IEnumerable<Office> offices)
        {
            try
            {
                if(!await IsOfficeAvailableToSet(PipelineConstName.CANTOR))
                {
                    return NotFound(new { message = ActionResultMessage.OfficeNotAdded });
                }
                if(IsCurrentUserCantor())
                {
                    await _officesManager.AddBrothersForSchola(offices);
                    await _officesManager.UpdatePipelineStatus(PipelineConstName.CANTOR, false);
                    await _officesManager.UpdatePipelineStatus(PipelineConstName.TRAY, true);
                    return Ok(new { message = ActionResultMessage.OfficeAdded });
                }
                return Unauthorized( new { message = ActionResultMessage.UnauthorizedUser });
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
            var client = _clientFactory.CreateClient("TrayCommunions");
            var officeHour = await client.GetFromJsonAsync<IEnumerable<string>>("hours-tray");
            return officeName.Union(officeHour);
        }

        [HttpPost("office-liturgist")]
        public async Task<ActionResult> AddLiturgistOffice(IEnumerable<Office> offices)
        {
            try
            {
                if (!await IsOfficeAvailableToSet(PipelineConstName.LITURGIST))
                {
                    return NotFound(new { message = ActionResultMessage.OfficeNotAdded });
                }
                if (IsCurrenUserLiturgist())
                {
                    await _officesManager.AddLiturgistOffice(offices);
                    await _officesManager.UpdatePipelineStatus(PipelineConstName.LITURGIST, false);
                    await _officesManager.UpdatePipelineStatus(PipelineConstName.DEAN, true);
                    return Ok(new { message = ActionResultMessage.OfficeAdded });
                }
                return Unauthorized(new { message = ActionResultMessage.UnauthorizedUser });
            }
            catch
            {
                return NotFound(new { message = ActionResultMessage.OfficeNotAdded });
            }
        }

        [HttpPost("office-dean")]
        public async Task<ActionResult> AddDeanOffice(IEnumerable<Office> offices)
        {
            try
            {
                var client = _clientFactory.CreateClient("Weeks");
                if (!await IsOfficeAvailableToSet(PipelineConstName.DEAN))
                {
                    return NotFound(new { message = ActionResultMessage.OfficeNotAdded });
                }
                if (IsCurrentUserDean())
                {
                    await _officesManager.AddDeanOffice(offices);
                    await client.GetFromJsonAsync<Task>("");
                    await _officesManager.UpdatePipelineStatus(PipelineConstName.DEAN, false);
                    await _officesManager.UpdatePipelineStatus(PipelineConstName.KITCHEN, true);
                    return Ok(new { message = ActionResultMessage.OfficeAdded });
                }
                return Unauthorized(new { message = ActionResultMessage.UnauthorizedUser });
            }
            catch
            {
                return NotFound(new { message = ActionResultMessage.OfficeNotAdded });
            }
        }


        [HttpGet("office-last/{brotherId}")]
        public async Task<OfficeBrother> GetLastOfficeForBrother(int brotherId)
        {
            var clientTrayCommunion = _clientFactory.CreateClient("TrayCommunions");
            var clientWeek = _clientFactory.CreateClient("Weeks");
            int weekNumber = await clientWeek.GetFromJsonAsync<int>("") - 1;
            var trays = await clientTrayCommunion.GetFromJsonAsync<IEnumerable<string>>($"tray-hour-last/{brotherId}");
            var communions = await clientTrayCommunion.GetFromJsonAsync<IEnumerable<string>>($"communion-hour-last/{brotherId}");
            var otherOffices = await _officesManager.GetOfficeForBrother(weekNumber, brotherId);
            return _officesManager.GetOfficeForSingleBrother(trays, communions, otherOffices);
        }

        [HttpGet("office-previous/{brotherId}")]
        public async Task<OfficeBrother> GetPreviousOfficeForBrother(int brotherId)
        {
            var clientTrayCommunion = _clientFactory.CreateClient("TrayCommunions");
            var clientWeek = _clientFactory.CreateClient("Weeks");
            int weekNumber = await clientWeek.GetFromJsonAsync<int>("") - 2;
            var trays = await clientTrayCommunion.GetFromJsonAsync<IEnumerable<string>>($"tray-hour-last/{brotherId}");
            var communions = await clientTrayCommunion.GetFromJsonAsync<IEnumerable<string>>($"communion-hour-last/{brotherId}");
            var otherOffices = await _officesManager.GetOfficeForBrother(weekNumber, brotherId);
            return _officesManager.GetOfficeForSingleBrother(trays, communions, otherOffices);
        }

        [HttpGet("office-last")]
        public async Task<IEnumerable<OfficePrint>> GetLastOffice()
        {
            var clientWeek = _clientFactory.CreateClient("Weeks");
            var clientBrother = _clientFactory.CreateClient("Brothers");
            var clientTrayCommunion = _clientFactory.CreateClient("TrayCommunions");
            int weekNumber = await clientWeek.GetFromJsonAsync<int>("") - 1;
            var brothers = (await clientBrother.GetFromJsonAsync<IEnumerable<Brother>>("")).OrderBy(bro => bro.Precedency);
            var trays = await clientTrayCommunion.GetFromJsonAsync<IEnumerable<TrayOfficeAdded>>($"tray-hour/{weekNumber}");
            var communions = await clientTrayCommunion.GetFromJsonAsync<IEnumerable<CommunionOfficeAdded>>($"communion-hour/{weekNumber}");
            var otherOffices = await _officesManager.GetOffice(weekNumber);
            return _officesManager.GetOfficeForAllBrothers(brothers, trays, communions, otherOffices);
        }

        [HttpPost("kitchen-offices")]
        public async Task<ActionResult> AddKitchenOffices(IEnumerable<KitchenOffices> offices)
        {
            try
            {
                if (!await IsOfficeAvailableToSet(PipelineConstName.KITCHEN))
                {
                    return NotFound(new { message = ActionResultMessage.OfficeNotAdded });
                }
                if (IsCurrentUserDean())
                {
                    await _officesManager.AddKitchenOffices(offices);
                    await _officesManager.UpdatePipelineStatus(PipelineConstName.KITCHEN, false);
                    await _officesManager.UpdatePipelineStatus(PipelineConstName.CANTOR, true);
                    await _officesManager.UpdatePipelineStatus(PipelineConstName.COMMUNION, true);
                    return Ok(new { message = ActionResultMessage.OfficeAdded });
                }
                return Unauthorized(new { message = ActionResultMessage.UnauthorizedUser });
            }
            catch
            {
                return NotFound(new { message = ActionResultMessage.OfficeNotAdded });
            }
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

        [HttpGet("offices-name")]
        public async Task<IEnumerable<string>> GetOfficesName()
        {
            return await _officesManager.GetOfficesName();
        }

        [HttpGet("pipeline-status/{name}")]
        public async Task<Boolean> IsOfficeAvailableToSet(string name)
        {
            return await _officesManager.GetPipelineStatus(name);
        }

        [HttpPut("pipeline")]
        public async Task UpdatePipelineStatus(PipelineUpdate pipelineUpdate)
        {
            await _officesManager.UpdatePipelineStatus(pipelineUpdate.JobName, pipelineUpdate.JobValue);
        }

        private BaseModel GetCurrentUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if(identity != null)
            {
                var userClaims = identity.Claims;
                return new BaseModel
                {
                    Id = Int32.Parse(userClaims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value ?? "0"),
                    Name = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.GivenName)?.Value,
                    Surname = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Surname)?.Value,
                    StatusBrother = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value
                };
            }
            return null;
        }

        private Boolean IsCurrentUserDean()
        {
            return GetCurrentUser().StatusBrother == BrotherStatus.DEAN;
        }

        private Boolean IsCurrentUserCantor()
        {
            return GetCurrentUser().StatusBrother == BrotherStatus.CANTOR;
        }

        private Boolean IsCurrenUserLiturgist()
        {
            return GetCurrentUser().StatusBrother == BrotherStatus.LITURGIST;
        }
    }
}
