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
        public async Task<IActionResult> Sale([FromRoute] string accessToken, [FromBody] GumroadSaleRequest request)
        {
            var action = IsRequestValid(accessToken, request.Resource_Name, "sale");
            if (action != null)
            {
                return action;
            }

            try
            {
                await gumroadService.RegisterLicense(request);
                return Ok("");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("deactivate/{accessToken}")]
        public async Task<IActionResult> Deactivate([FromRoute] string accessToken, GumroadDeactivateRequest request)
        {
            var action = IsRequestValid(accessToken, request.Resource_Name, "subscription_ended");
            if (action != null)
            {
                return action;
            }

            try
            {
                await gumroadService.DeactivateLicense(request);
                return Ok("");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("reactivate/{accessToken}")]
        public async Task<IActionResult> Reactivate([FromRoute] string accessToken, GumroadReactivateRequest request)
        {
            var action = IsRequestValid(accessToken, request.Resource_Name, "subscription_restarted");
            if (action != null)
            {
                return action;
            }

            try
            {
                await gumroadService.ReactivateLicense(request);
                return Ok("");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("update/{accessToken}")]
        public async Task<IActionResult> Updated([FromRoute] string accessToken, GumroadUpdateRequest request)
        {
            var action = IsRequestValid(accessToken, request.Resource_Name, "subscription_updated");
            if (action != null)
            {
                return action;
            }

            try
            {
                await gumroadService.UpdateLicense(request);
                return Ok("");
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
