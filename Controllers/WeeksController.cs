using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using VirtualDean.Data;

namespace VirtualDean.Controllers
{
    [Route("api/[controller]")]
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
