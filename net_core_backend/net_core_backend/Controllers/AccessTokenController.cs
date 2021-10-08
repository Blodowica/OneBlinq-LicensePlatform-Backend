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
    public class AccessTokenController : ControllerBase
    {
        private readonly IAccessTokenService accessTokenService;

        public AccessTokenController(IAccessTokenService accessTokenService)
        {
            this.accessTokenService = accessTokenService;
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("create-access-token")]
        public async Task<IActionResult> CreateAccessToken([FromBody] CreateAccessTokenRequest model)
        {
            try
            {
                var response = await accessTokenService.CreateAccessToken();

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
