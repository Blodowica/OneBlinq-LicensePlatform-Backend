using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using net_core_backend.Helpers;
using net_core_backend.Models;
using net_core_backend.Models.GumroadRequests;
using net_core_backend.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace net_core_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GumroadController : ControllerBase
    {
        private readonly IGumroadService gumroadService;
        private readonly IAccessTokenService accessTokenService;

        public GumroadController(IGumroadService gumroadService, IAccessTokenService accessTokenService)
        {
            this.gumroadService = gumroadService;
            this.accessTokenService = accessTokenService;
        }

        [HttpPost("sale/{accessToken}")]
        public async Task<IActionResult> Sale([FromRoute] string accessToken, [FromForm] GumroadSaleRequest request)
        {
            try
            {
                await accessTokenService.CheckAccessToken(accessToken);
                request.variants = HttpContext.Request.Form["variants[Tier]"];
                await gumroadService.RegisterLicense(request);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("deactivate/{accessToken}")]
        public async Task<IActionResult> Deactivate([FromRoute] string accessToken, [FromForm] GumroadDeactivateRequest request)
        {
            try
            {
                await accessTokenService.CheckAccessToken(accessToken);
                await gumroadService.DeactivateLicense(request);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("reactivate/{accessToken}")]
        public async Task<IActionResult> Reactivate([FromRoute] string accessToken, [FromForm] GumroadReactivateRequest request)
        {
            try
            {
                await accessTokenService.CheckAccessToken(accessToken);
                await gumroadService.ReactivateLicense(request);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("update/{accessToken}")]
        public async Task<IActionResult> Updated([FromRoute] string accessToken, [FromForm] GumroadUpdateRequest request)
        {
            try
            {
                await accessTokenService.CheckAccessToken(accessToken);
                await gumroadService.UpdateLicense(request);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("cancel/{accessToken}")]
        public async Task<IActionResult> Cancel([FromRoute] string accessToken, [FromForm] GumroadCancelRequest request)
        {
            try
            {
                await accessTokenService.CheckAccessToken(accessToken);
                await gumroadService.CancelLicense(request);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
