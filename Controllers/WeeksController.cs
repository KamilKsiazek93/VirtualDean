using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VirtualDean.Data;

namespace VirtualDean.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class WeeksController : ControllerBase
    {
        private readonly IWeek _week;

        public WeeksController(IWeek week)
        {
            _week = week;
        }

        [HttpGet]
        public async Task<int> GetActualWeekNumber()
        {
            return await _week.GetLastWeek();
        }

        [HttpPost]
        public async Task IncrementWeek()
        {
            await _week.IncrementWeek();
        }
    }
}
