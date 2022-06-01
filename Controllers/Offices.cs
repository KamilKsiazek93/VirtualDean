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
        private readonly IWeek _week;
        
        public Offices(IBrothers brothers, IOfficesManager officesManager,
            ITrayCommunionHour trayCommunionHour, IObstacle obstacle, IWeek week)
        {
            _brothers = brothers;
            _officesManager = officesManager;
            _trayCommunionHour = trayCommunionHour;
            _obstacle = obstacle;
            _week = week;
        }

        [HttpGet("brothers")]
        public async Task<IEnumerable<Brother>> GetBrothers()
        {
            return await _brothers.GetBrothers();
        }

        [AllowAnonymous]
        [HttpGet("brother-login")]
        public async Task<BaseModel> LoginAction([FromQuery]LoginModel loginData)
        {
            var findingBrother =  await _brothers.FindLoginBrother(loginData);
            if(findingBrother != null)
            {

                return _brothers.GetAuthenticatedBrother(findingBrother);
            }
            return null;
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

        [HttpDelete("brothers/{id}")]
        public async Task<ActionResult> DeleteBrother(int id)
        {
            var brother = await _brothers.GetBrother(id);
            if(brother == null)
            {
                return NotFound(new { message = ActionResultMessage.BrotherNotFound });
            }
            await _brothers.DeleteBrother(brother);
            return Ok(new { message = ActionResultMessage.BrotherDeleted });
        }

        [HttpPut("brothers/{id}")]
        public async Task<IActionResult> EditBrother(Brother brother)
        {
            try
            {
                await _brothers.EditBrother(brother);
            }
            catch(Exception ex)
            {
                NotFound( new { message = ActionResultMessage.OperationFailed, ex});
            }
            return Ok(new { message = ActionResultMessage.DataUpdated });
        }

        [HttpPost("brothers")]
        public async Task<ActionResult<Brother>> AddBrothers(Brother brother)
        {
            if(!await _brothers.IsBrotherInDb(brother))
            {
                 await _brothers.SaveBrother(brother);
                return Ok(new { status = 201, isSucces = true, message = "Dodano nowego brata" });
            }
            return Ok(new { status = 401, isSuucces = false, message = ActionResultMessage.BrotherAlreadyExist }) ;
        }

        [HttpGet("brothers-tray")]
        public async Task<IEnumerable<BaseModel>> GetBrothersForTray()
        {
            return await _brothers.GetBrothersForTray();
        }

        [HttpGet("brothers-communion")]
        public async Task<IEnumerable<BaseModel>> GetBrothersForCommunion()
        {
            return await _brothers.GetBrothersForCommunion();
        }

        [HttpGet("brothers-liturgistOffice")]
        public async Task<IEnumerable<BaseBrotherForLiturgistOffice>> GetBrothersForLiturgistOffice()
        {
            return await _brothers.GetBrotherForLiturgistOffice();
        }

        [HttpGet("brothers-singing")]
        public async Task<IEnumerable<BaseModel>> GetSingingBrothers()
        {
            return await _brothers.GetSingingBrothers();
        }

        [HttpPost("office-singing")]
        public async Task<ActionResult> AddScholaOffices(IEnumerable<Office> offices)
        {
            try
            {
                await _officesManager.AddBrothersForSchola(offices);
                return Ok(new { message = ActionResultMessage.OfficeAdded });
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

        [HttpPost("office-liturgist")]
        public async Task<ActionResult> AddLiturgistOffice(IEnumerable<Office> offices)
        {
            try
            {
                await _officesManager.AddLiturgistOffice(offices);
                return Ok(new { message = ActionResultMessage.OfficeAdded });
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
                await _officesManager.AddDeanOffice(offices);
                await _week.IncrementWeek();
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
            int weekNumber = await _week.GetLastWeek();
            var trays = await _trayCommunionHour.GetTrayHour(weekNumber, brotherId);
            var communions = await _trayCommunionHour.GetCommunionHour(weekNumber, brotherId);
            var otherOffices = await _officesManager.GetOfficeForBrother(weekNumber, brotherId);
            return new OfficeBrother
            {
                BrotherId = brotherId,
                CantorOffice = otherOffices.CantorOffice,
                Tray = trays,
                Communion = communions,
                LiturgistOffice = otherOffices.LiturgistOffice,
                DeanOffice = otherOffices.DeanOffice
            };
        }

        [HttpGet("office-previous/{brotherId}")]
        public async Task<OfficeBrother> GetPreviousOfficeForBrother(int brotherId)
        {
            int weekNumber = await _week.GetLastWeek() - 1;
            var trays = await _trayCommunionHour.GetTrayHour(weekNumber, brotherId);
            var communions = await _trayCommunionHour.GetCommunionHour(weekNumber, brotherId);
            var otherOffices = await _officesManager.GetOfficeForBrother(weekNumber, brotherId);
            return new OfficeBrother
            {
                BrotherId = brotherId,
                CantorOffice = otherOffices.CantorOffice,
                Tray = trays,
                Communion = communions,
                LiturgistOffice = otherOffices.LiturgistOffice,
                DeanOffice = otherOffices.DeanOffice
            };
        }

        [HttpGet("office-last")]
        public async Task<IEnumerable<Office>> GetLastOffice()
        {
            return await _officesManager.GetLastOffice();
        }

        [HttpPost("kitchen-offices")]
        public async Task<ActionResult<KitchenOffices>> AddKitchenOffices(IEnumerable<KitchenOffices> offices)
        {
            await _officesManager.AddKitchenOffices(offices);
            return CreatedAtAction(nameof(GetKitchenOffices), offices);
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
                await _trayCommunionHour.AddTrayHour(listOfTray);
                return Ok(new { message = ActionResultMessage.OfficeAdded });
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
            try
            {
                await _trayCommunionHour.AddCommunionHour(listOfCommunion);
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

        [HttpPost("obstacles")]
        public async Task<ActionResult> AddObstacles(IEnumerable<ObstaclesAdded> obstacles)
        {
            try
            {
                await _obstacle.AddObstacle(obstacles);
                return Ok(new { message = ActionResultMessage.OfficeAdded });
            }
            catch
            {
                return NotFound(new { message = ActionResultMessage.OperationFailed });
            }
        }

        [HttpGet("obstacle-last")]
        public async Task<IEnumerable<ObstaclesList>> GetLastObstacleAdded()
        {
            return await _obstacle.GetLastObstacleAdded();
        }

        [HttpGet("obstacles/{weekId}")]
        public async Task<IEnumerable<ObstaclesList>> GetObstaclesInWeek(int weekId)
        {
            return await _obstacle.GetObstacles(weekId);
        }

        [HttpPost("obstacle-const")]
        public async Task<ActionResult> AddConstObstacle(ConstObstacleAdded obstacles)
        {
            try
            {
                await _obstacle.AddConstObstacle(obstacles);
                return Ok(new { message = ActionResultMessage.ObstacleAdded });
            }
            catch
            {
                return NotFound(new { message = ActionResultMessage.OperationFailed });
            }
        }

        [HttpGet("obstacle-const/{brotherId}")]
        public async Task<IEnumerable<string>> GetConstObstacle(int brotherId)
        {
            return await _obstacle.GetConstObstacleForBrother(brotherId);
        }


        [HttpGet("obstacle-const")]
        public async Task<IEnumerable<ConstObstacleAdded>> GetAllObstaclesConst()
        {
            return await _obstacle.GetAllConstObstacles();
        }

        [HttpGet("obstacle-const/brothers-data")]
        public async Task<IEnumerable<ConstObstacleWithBrotherData>> GetObstacleWithBrotherData()
        {
            var baseBrothers = await _brothers.GetBaseBrothersModel();
            return await _obstacle.GetObstacleWithBrotherData(baseBrothers);
        }

        [HttpDelete("obstacle-const/{id}")]
        public async Task<ActionResult> DeleteConstObstacle(int id)
        {
            var obstacle = await _obstacle.GetConstObstacle(id);
            if (obstacle == null)
            {
                return NotFound(new { message = ActionResultMessage.ObstacleNotFound });
            }
            await _obstacle.DeleteConstObstacle(obstacle);
            return Ok(new { message = ActionResultMessage.ObstacleDeleted });
        }

        [HttpPut("obstacle-const/{id}")]
        public async Task<IActionResult> EditOBstacleConst(ConstObstacleAdded obstacle)
        {
            try
            {
                await _obstacle.EditConstObstacle(obstacle);
            }
            catch (Exception ex)
            {
                NotFound(new { message = ActionResultMessage.OperationFailed, ex });
            }
            return Ok(new { message = ActionResultMessage.DataUpdated });
        }

        [HttpGet("offices-name")]
        public async Task<IEnumerable<string>> GetOfficesName()
        {
            return await _officesManager.GetOfficesName();
        }

        [HttpGet("obstacle-between-office")]
        public async Task<IEnumerable<ObstacleBetweenOffice>> GetObstacleBetweenOffices()
        {
            return await _obstacle.GetObstacleBetweenOffices();
        }

        [HttpPost("obstacle-between-office")]
        public async Task<ActionResult> AddObstacleBetweenOffice(ObstacleBetweenOffice obstacle)
        {
            try
            {
                await _obstacle.AddObstacleBetweenOffices(obstacle);
                return Ok(new { message = ActionResultMessage.ObstacleAdded });
            }
            catch
            {
                return NotFound(new { message = ActionResultMessage.ObstacleNotAdded });
            }
        }

        [HttpPut("obstacle-between-office/{id}")]
        public async Task<ActionResult> EditObstacleBetweenOffice(ObstacleBetweenOffice obstacle)
        {
            try
            {
                await _obstacle.EditObstacleBetweenOffices(obstacle);
            }
            catch
            {
                NotFound(new { message = ActionResultMessage.OperationFailed });
            }
            return Ok(new { message = ActionResultMessage.DataUpdated });
        }

        [HttpDelete("obstacle-between-office/{id}")]
        public async Task<ActionResult> DeleteObstacleBetweenOffice(int id)
        {
            var obstacle = await _obstacle.GetObstacleBetweenOffice(id);
            if (obstacle == null)
            {
                return NotFound(new { message = ActionResultMessage.ObstacleNotFound });
            }
            await _obstacle.DeleteObstacleBetweenOffices(obstacle);
            return Ok(new { message = ActionResultMessage.ObstacleDeleted });
        }

        [HttpGet("pipeline-cantor")]
        public async Task<Boolean> IsCantorOfficeAvailableToSet()
        {
            return await _officesManager.IsKitchenOfficeAlreadySet() && !await _officesManager.IsScholaAlreadySet();
        }

        [HttpGet("pipeline-tray")]
        public async Task<Boolean> IsTrayAvailableToSet()
        {
            return await _officesManager.IsScholaAlreadySet() && !await _trayCommunionHour.IsTrayAlreadySet(); ;
        }

        [HttpGet("pipeline-liturgist")]
        public async Task<Boolean> IsLiturgistOfficeAvailableToSet()
        {
            return await _trayCommunionHour.IsTrayAlreadySet() && !await _officesManager.IsLiturgistOfficeAlreadySet(); ;
        }

        [HttpGet("pipeline-dean")]
        public async Task<Boolean> IsDeanOfficeAvailableToSet()
        {
            int weekNumber = await _week.GetLastWeek();
            return await _officesManager.IsLiturgistOfficeAlreadySet() && !await _officesManager.IsDeanOfficeAlreadySet(weekNumber); ;
        }

        [HttpGet("pipeline-kitchen")]
        public async Task<Boolean> IsKitchenOfficeAvailableToSet()
        {
            int weekNumber = await _week.GetLastWeek() - 1;
            return await _officesManager.IsDeanOfficeAlreadySet(weekNumber) && !await _officesManager.IsKitchenOfficeAlreadySet();
        }
    }
}
