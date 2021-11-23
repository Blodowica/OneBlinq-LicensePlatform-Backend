using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using net_core_backend.Models;
using net_core_backend.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FreeTrialController : ControllerBase
    {
        private readonly IAccessTokenService accessTokenService;
        private readonly IFreeTrialService freeTrialService;

        public FreeTrialController(IAccessTokenService accessTokenService, IFreeTrialService freeTrialService)
        {
            this.accessTokenService = accessTokenService;
            this.freeTrialService = freeTrialService;
        }

        [HttpPost("get-free-trial/{accessToken}")]
        public async Task<IActionResult> CreateFreeTrial([FromRoute] String accessToken, [FromBody] FreeTrialRequest model)
        {
            try
            {
                await accessTokenService.CheckAccessToken(accessToken);
                await freeTrialService.CreateFreeTrial(model);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("verify-free-trial/{accessToken}")]
        public async Task<IActionResult> VerifyFreeTrial([FromRoute] String accessToken, [FromBody] FreeTrialRequest model)
        {
            try
            {
                await accessTokenService.CheckAccessToken(accessToken);
                await freeTrialService.VerifyFreeTrial(model);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
