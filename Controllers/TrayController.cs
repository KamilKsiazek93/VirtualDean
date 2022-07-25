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
    public class TrayController : ControllerBase
    {
        private readonly ITrayCommunionHour _trayCommunionHour;
        private readonly IHttpClientFactory _clientFactory;

        public TrayController(ITrayCommunionHour trayCommunionHour, IHttpClientFactory clientFactory)
        {
            _trayCommunionHour = trayCommunionHour;
            _clientFactory = clientFactory;
        }
    }
}
