using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;
using VirtualDean.Data;
using VirtualDean.Enties;
using VirtualDean.Models;

namespace VirtualDean.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ObstaclesController : ControllerBase
    {
        private readonly IObstacle _obstacle;
        private readonly IHttpClientFactory _clientFactory;
        public ObstaclesController(IObstacle obstacle, IHttpClientFactory clientFactory)
        {
            _obstacle = obstacle;
            _clientFactory = clientFactory;
        }

        [HttpPost]
        public async Task<ActionResult> AddObstacles(IEnumerable<ObstaclesAdded> obstacles)
        {
            try
            {
                var user = GetCurrentUser();
                if (user.Id == obstacles.FirstOrDefault().BrotherId)
                {
                    await _obstacle.AddObstacle(obstacles);
                    return Ok(new { message = ActionResultMessage.ObstacleAdded });
                }
                return Unauthorized(new { message = ActionResultMessage.UnauthorizedUser });
            }
            catch
            {
                return NotFound(new { message = ActionResultMessage.OperationFailed });
            }
        }

        [HttpGet("last")]
        public async Task<IEnumerable<ObstaclesList>> GetLastObstacleAdded()
        {
            return await _obstacle.GetLastObstacleAdded();
        }

        [HttpGet("{weekId}")]
        public async Task<IEnumerable<ObstaclesList>> GetObstaclesInWeek(int weekId)
        {
            return await _obstacle.GetObstacles(weekId);
        }

        [Authorize(Policy = "Dean")]
        [HttpPost("const")]
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

        [HttpGet("const/{brotherId}")]
        public async Task<IEnumerable<string>> GetConstObstacle(int brotherId)
        {
            return await _obstacle.GetConstObstacleForBrother(brotherId);
        }

        [HttpGet("const")]
        public async Task<IEnumerable<ConstObstacleAdded>> GetAllObstaclesConst()
        {
            return await _obstacle.GetAllConstObstacles();
        }

        [HttpGet("const/brothers-data")]
        public async Task<IEnumerable<ConstObstacleWithBrotherData>> GetObstacleWithBrotherData()
        {
            var client = _clientFactory.CreateClient("Brothers");
            var baseBrothers = await client.GetFromJsonAsync<IEnumerable<BaseModel>>("base");
            return await _obstacle.GetObstacleWithBrotherData(baseBrothers);
        }

        [Authorize(Policy = "Dean")]
        [HttpDelete("const/{id}")]
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

        [Authorize(Policy = "Dean")]
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

        [HttpGet("offices")]
        public async Task<IEnumerable<ObstacleBetweenOffice>> GetObstacleBetweenOffices()
        {
            return await _obstacle.GetObstacleBetweenOffices();
        }

        [Authorize(Policy = "Dean")]
        [HttpPost("offices")]
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

        [Authorize(Policy = "Dean")]
        [HttpPut("offices/{id}")]
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

        [Authorize(Policy = "Dean")]
        [HttpDelete("offices/{id}")]
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

        private BaseModel GetCurrentUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity != null)
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
    }
}
