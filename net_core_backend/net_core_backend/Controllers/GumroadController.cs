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

        public GumroadController(IGumroadService gumroadService)
        {
            this.gumroadService = gumroadService;
        }

        [HttpPost("sale/{accessToken}")]
        public async Task<IActionResult> Sale([FromRoute] string accessToken, [FromForm] GumroadSaleRequest request)
        {
            request.variants = HttpContext.Request.Form["variants[Tier]"];
            try
            {
                await gumroadService.RegisterLicense(accessToken, request);
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
                await gumroadService.DeactivateLicense(accessToken, request);
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
                await gumroadService.ReactivateLicense(accessToken, request);
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
                await gumroadService.UpdateLicense(accessToken, request);
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
                await gumroadService.CancelLicense(accessToken, request);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
