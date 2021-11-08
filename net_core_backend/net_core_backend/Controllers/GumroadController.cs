using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using net_core_backend.Helpers;
using net_core_backend.Models;
using net_core_backend.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GumroadController : ControllerBase
    {
        private readonly IGumroadService gumroadService;
        private readonly AppSettings appSettings;

        public GumroadController(IGumroadService gumroadService, IOptions<AppSettings> appSettings)
        {
            this.gumroadService = gumroadService;
            this.appSettings = appSettings.Value;
        }

        [HttpPost("sale/{accessToken}")]
        public async Task<IActionResult> Sale([FromRoute] string accessToken, [FromForm] GumroadSaleRequest request)
        {
            request.variants = HttpContext.Request.Form["variants[Tier]"];
            var action = IsRequestValid(accessToken, request.resource_name, "sale");
            if (action != null)
            {
                return action;
            }

            try
            {
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
            var action = IsRequestValid(accessToken, request.resource_name, "subscription_ended");
            if (action != null)
            {
                return action;
            }

            try
            {
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
            var action = IsRequestValid(accessToken, request.resource_Name, "subscription_restarted");
            if (action != null)
            {
                return action;
            }

            try
            {
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
            var action = IsRequestValid(accessToken, request.resource_name, "subscription_updated");
            if (action != null)
            {
                return action;
            }

            try
            {
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
            var action = IsRequestValid(accessToken, request.resource_name, "cancellation");
            if (action != null)
            {
                return action;
            }

            try
            {
                await gumroadService.CancelLicense(request);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private IActionResult IsRequestValid(string accessToken, string requestResourceName, string resourceName)
        {
            if (accessToken != appSettings.GumroadAccessToken)
            {
                return Unauthorized("Incorrect accesstoken, request denied.");
            }

            if (requestResourceName != resourceName)
            {
                return BadRequest("Incorrect resource.");
            }
            return null;
        }
    }
}
