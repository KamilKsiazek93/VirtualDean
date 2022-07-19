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
        private readonly ITrayCommunionHour _trayCommunionHour;
        private readonly IObstacle _obstacle;
        private readonly IWeek _week;
        
        public OfficesController(IHttpClientFactory clientFactory, IOfficesManager officesManager,
            ITrayCommunionHour trayCommunionHour, IObstacle obstacle, IWeek week)
        {
            _clientFactory = clientFactory;
            _officesManager = officesManager;
            _trayCommunionHour = trayCommunionHour;
            _obstacle = obstacle;
            _week = week;
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

        [HttpGet("week-number")]
        public async Task<int> GetActualWeekNumber()
        {
            return await _week.GetLastWeek();
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
            var officeHour = await _trayCommunionHour.GetHoursForTray();
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
                if (!await IsOfficeAvailableToSet(PipelineConstName.DEAN))
                {
                    return NotFound(new { message = ActionResultMessage.OfficeNotAdded });
                }
                if (IsCurrentUserDean())
                {
                    await _officesManager.AddDeanOffice(offices);
                    await _week.IncrementWeek();
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
            int weekNumber = await _week.GetLastWeek() - 1;
            var trays = await _trayCommunionHour.GetTrayHour(weekNumber, brotherId);
            var communions = await _trayCommunionHour.GetCommunionHour(weekNumber, brotherId);
            var otherOffices = await _officesManager.GetOfficeForBrother(weekNumber, brotherId);
            return _officesManager.GetOfficeForSingleBrother(trays, communions, otherOffices);
        }

        [HttpGet("office-previous/{brotherId}")]
        public async Task<OfficeBrother> GetPreviousOfficeForBrother(int brotherId)
        {
            int weekNumber = await _week.GetLastWeek() - 2;
            var trays = await _trayCommunionHour.GetTrayHour(weekNumber, brotherId);
            var communions = await _trayCommunionHour.GetCommunionHour(weekNumber, brotherId);
            var otherOffices = await _officesManager.GetOfficeForBrother(weekNumber, brotherId);
            return _officesManager.GetOfficeForSingleBrother(trays, communions, otherOffices);
        }

        [HttpGet("office-last")]
        public async Task<IEnumerable<OfficePrint>> GetLastOffice()
        {
            var clientBrother = _clientFactory.CreateClient("Brothers");
            int weekNumber = await _week.GetLastWeek() - 1;
            var brothers = (await clientBrother.GetFromJsonAsync<IEnumerable<Brother>>("")).OrderBy(bro => bro.Precedency);
            var trays = await _trayCommunionHour.GetTrayHours(weekNumber);
            var communions = await _trayCommunionHour.GetCommunionHours(weekNumber);
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

        [HttpPost("tray-hour")]
        public async Task<ActionResult> AddTrayOffice(IEnumerable<TrayOfficeAdded> listOfTray)
        {
            try
            {
                if (!await IsOfficeAvailableToSet(PipelineConstName.TRAY))
                {
                    return NotFound(new { message = ActionResultMessage.OfficeNotAdded });
                }
                if (IsCurrenUserLiturgist())
                {
                    await _trayCommunionHour.AddTrayHour(listOfTray);
                    await _officesManager.UpdatePipelineStatus(PipelineConstName.TRAY, false);
                    await _officesManager.UpdatePipelineStatus(PipelineConstName.LITURGIST, true);
                    return Ok(new { message = ActionResultMessage.OfficeAdded });
                }
                return Unauthorized( new { message = ActionResultMessage.UnauthorizedUser });
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
            int weekNumber = await _week.GetLastWeek();
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
                await _trayCommunionHour.AddCommunionHour(listOfCommunion);
                await _officesManager.UpdatePipelineStatus(PipelineConstName.COMMUNION, false);
                return Ok(new { message = ActionResultMessage.OfficeAdded });
            }
            catch
            {
                return NotFound(new { message = ActionResultMessage.OperationFailed });
            }
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
            int weekNumber = await _week.GetLastWeek();
            return await _trayCommunionHour.GetCommunionHour(weekNumber, brotherId);
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
