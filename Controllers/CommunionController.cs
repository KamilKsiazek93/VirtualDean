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
    public class CommunionController : ControllerBase
    {
        private readonly ICommunionRepository _communionRepository;
        private readonly IHttpClientFactory _clientFactory;

        public CommunionController(ICommunionRepository communionRepository, IHttpClientFactory clientFactory)
        {
            _communionRepository = communionRepository;
            _clientFactory = clientFactory;
        }

        [HttpPost]
        public async Task<ActionResult> AddCommunionOffice(IEnumerable<CommunionOfficeAdded> listOfCommunion)
        {
            if (!await IsOfficeAvailableToSet(PipelineConstName.COMMUNION))
            {
                return NotFound(new { message = ActionResultMessage.OfficeNotAdded });
            }
            try
            {
                var client = _clientFactory.CreateClient("Offices");
                await _communionRepository.AddCommunionHour(listOfCommunion);
                await client.PutAsJsonAsync("pipeline", new PipelineUpdate { JobName = PipelineConstName.COMMUNION, JobValue = false });
                return Ok(new { message = ActionResultMessage.OfficeAdded });
            }
            catch
            {
                return NotFound(new { message = ActionResultMessage.OperationFailed });
            }
        }

        [HttpGet]
        public async Task<IEnumerable<CommunionOfficeAdded>> GetCommunionHour()
        {
            return await _communionRepository.GetCommunionHours();
        }

        [HttpGet("week/{weekId}")]
        public async Task<IEnumerable<CommunionOfficeAdded>> GetCommunionHour(int weekId)
        {
            return await _communionRepository.GetCommunionHours(weekId);
        }

        [HttpGet("brother/{brotherId}")]
        public async Task<IEnumerable<string>> GetLastCommunionForBrother(int brotherId)
        {
            var client = _clientFactory.CreateClient("Weeks");
            int weekNumber = await client.GetFromJsonAsync<int>("");
            return await _communionRepository.GetCommunionHour(weekNumber, brotherId);
        }

        [HttpGet("hours")]
        public Task<IEnumerable<string>> GetHoursForCommunion()
        {
            return _communionRepository.GetHoursForCommunion();
        }

        private Task<bool> IsOfficeAvailableToSet(string officeName)
        {
            var client = _clientFactory.CreateClient("Offices");
            return client.GetFromJsonAsync<bool>($"pipeline-status/{officeName}");
        }
    }
}
