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
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class AccessTokenController : ControllerBase
    {
        private readonly IAccessTokenService accessTokenService;
        private readonly IPaginationService paginationService;

        public AccessTokenController(IAccessTokenService accessTokenService, IPaginationService PaginationService)
        {
            this.accessTokenService = accessTokenService;
            this.paginationService = PaginationService;
        }

        [HttpPost("create-access-token")]
        public async Task<IActionResult> CreateAccessToken()
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

        [HttpPost("toggle-access-token/{accessTokenId}")]
        public async Task<IActionResult> ToggleAccessToken([FromRoute] string accessTokenId)
        {
            try
            {
                await accessTokenService.ToggleAccessToken(Convert.ToInt32(accessTokenId));

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
