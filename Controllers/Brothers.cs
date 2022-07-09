using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using VirtualDean.Data;
using VirtualDean.Enties;
using VirtualDean.Models;

namespace VirtualDean.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Brothers : ControllerBase
    {
        private readonly IBrothers _brothers;
        public Brothers(IBrothers brothers)
        {
            _brothers = brothers;
        }

        [HttpGet]
        public async Task<IEnumerable<Brother>> GetBrothers()
        {
            return await _brothers.GetBrothers();
        }

        [HttpGet("login")]
        public async Task<BaseModel> LoginAction([FromQuery] LoginModel loginData)
        {
            var findingBrother = await _brothers.FindLoginBrother(loginData);
            if (findingBrother != null)
            {

                return _brothers.GetAuthenticatedBrother(findingBrother);
            }
            return new BaseModel();
        }

        [HttpGet("base")]
        public async Task<IEnumerable<BaseModel>> GetBaseBrothersModel()
        {
            return await _brothers.GetBaseBrothersModel();
        }

        [HttpGet("{id}")]
        public async Task<Brother> GetBrothers(int id)
        {
            return await _brothers.GetBrother(id);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBrother(int id)
        {
            var brother = await _brothers.GetBrother(id);
            if (brother == null)
            {
                return NotFound(new { message = ActionResultMessage.BrotherNotFound });
            }
            await _brothers.DeleteBrother(brother);
            return Ok(new { message = ActionResultMessage.BrotherDeleted });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditBrother(Brother brother)
        {
            try
            {
                await _brothers.EditBrother(brother);
            }
            catch (Exception ex)
            {
                NotFound(new { message = ActionResultMessage.OperationFailed, ex });
            }
            return Ok(new { message = ActionResultMessage.DataUpdated });
        }

        [HttpPost("brothers")]
        public async Task<ActionResult<Brother>> AddBrothers(Brother brother)
        {
            if (!await _brothers.IsBrotherInDb(brother))
            {
                await _brothers.SaveBrother(brother);
                return Ok(new { status = 201, isSucces = true, message = "Dodano nowego brata" });
            }
            return NotFound(new { isSuucces = false, message = ActionResultMessage.BrotherAlreadyExist });
        }

        [HttpGet("tray")]
        public async Task<IEnumerable<BaseModel>> GetBrothersForTray()
        {
            return await _brothers.GetBrothersForTray();
        }

        [HttpGet("communion")]
        public async Task<IEnumerable<BaseModel>> GetBrothersForCommunion()
        {
            return await _brothers.GetBrothersForCommunion();
        }

        [HttpGet("liturgistOffice")]
        public async Task<IEnumerable<BaseBrotherForLiturgistOffice>> GetBrothersForLiturgistOffice()
        {
            return await _brothers.GetBrotherForLiturgistOffice();
        }

        [HttpGet("singing")]
        public async Task<IEnumerable<BaseModel>> GetSingingBrothers()
        {
            return await _brothers.GetSingingBrothers();
        }
    }
}
