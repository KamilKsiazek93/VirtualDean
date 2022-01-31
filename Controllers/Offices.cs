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

        [HttpGet("brother-login")]
        public async Task<BaseModel> LoginAction([FromQuery]LoginModel loginData)
        {
            var findingBrother =  await _brothers.FindLoginBrother(loginData);
            if(findingBrother != null)
            {
                return findingBrother;
            }
            return new BaseModel { };
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
                return NotFound(new { message = "Nie ma takiego brata w bazie danych" });
            }
            await _brothers.DeleteBrother(brother);
            return Ok(new { message = "Brat został usunięty" });
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
                NotFound( new { message = "Operacja się nie powiodła ", ex});
            }
            return Ok(new { message = "Zaktualizowano dane"});
        }

        [HttpPost("brothers")]
        public async Task<ActionResult<Brother>> AddBrothers(Brother brother)
        {
            if(!await _brothers.IsBrotherInDb(brother))
            {
                 await _brothers.SaveBrother(brother);
                return Ok(new { status = 201, isSucces = true, message = "Dodano nowego brata" });
            }
            return Ok(new { status = 401, isSuucces = false, message = "W bazie istnieje już brat o takim imieniu i nazwisku" }) ;
        }

        [HttpGet("brothers-tray")]
        public async Task<IEnumerable<BaseModel>> GetBrothersForTray()
        {
            return await _brothers.GetBrothersForTray();
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
                return Ok(new { message = "Dodano oficja" });
            }
            catch
            {
                return NotFound(new { message = "Nie udało się dodać oficjów" });
            }
        }

        [HttpGet("office-last/{brotherId}")]
        public async Task<Office> GetLastOfficeForBrother(int brotherId)
        {
            var office = new Office();
            return office;
        }

        [HttpPost("kitchen-offices")]
        public async Task<ActionResult<KitchenOfficeAdded>> AddKitchenOffices(IEnumerable<KitchenOfficeAdded> offices)
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
        public async Task<ActionResult> AddObstacles(IEnumerable<ObstaclesAdded> obstacles)
        {
            try
            {
                await _obstacle.AddObstacle(obstacles);
                return Ok(new { message = "Pomyślnie dodano przeszkody" });
            }
            catch
            {
                return NotFound(new { message = "Nie udało się dodać przeszkód" });
            }
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
                return Ok(new { message = "Dodano przeszkodę"});
            }
            catch
            {
                return NotFound(new { message = "Nie udało się dodać przeszkody" });
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
                return NotFound(new { message = "Nie ma takiej przeszkody" });
            }
            await _obstacle.DeleteConstObstacle(obstacle);
            return Ok(new { message = "Przeszkoda została usunięta" });
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
                NotFound(new { message = "Operacja się nie powiodła ", ex });
            }
            return Ok(new { message = "Zaktualizowano dane" });
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

        [HttpPost]
        public async Task<ActionResult> AddObstacleBetweenOffice(ObstacleBetweenOffice obstacle)
        {
            try
            {
                await _obstacle.AddObstacleBetweenOffices(obstacle);
                return Ok(new { message = "Dodano przeszkodę" });
            }
            catch
            {
                return NotFound(new { message = "Nie udało się dodać przeszkody" });
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
                NotFound(new { message = "Operacja się nie powiodła " });
            }
            return Ok(new { message = "Zaktualizowano dane" });
        }

        [HttpDelete("obstacle-between-office/{id}")]
        public async Task<ActionResult> DeleteObstacleBetweenOffice(int id)
        {
            var obstacle = await _obstacle.GetObstacleBetweenOffice(id);
            if (obstacle == null)
            {
                return NotFound(new { message = "Nie ma takiej przeszkody" });
            }
            await _obstacle.DeleteObstacleBetweenOffices(obstacle);
            return Ok(new { message = "Przeszkoda została usunięta" });
        }
    }
}
