using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using net_core_backend.Models;
using net_core_backend.Services;
using net_core_backend.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace net_core_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UniqueUserController : ControllerBase
    {
        private readonly IUniqueUserService uniqueUserService;
        private readonly IAccessTokenService accessTokenService;

        public UniqueUserController (IUniqueUserService _uniqueUserService, IAccessTokenService _accessTokenService)
        {
            uniqueUserService = _uniqueUserService;
            accessTokenService = _accessTokenService;
        }
        
        [HttpGet("create-id/{accessToken}")]
        public async Task<IActionResult> CreateUniqueId([FromRoute] string accessToken)
        {
            try
            {
                await accessTokenService.CheckAccessToken(accessToken);
                var response = await uniqueUserService.CreateId();

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
