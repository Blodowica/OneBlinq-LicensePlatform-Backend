using Microsoft.AspNetCore.Http;
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
    public class LicenseKeyController : ControllerBase
    {
        private readonly ILicenseKeyService licenseKeyService;
        private readonly ILoggingService loggingService;

        public LicenseKeyController(ILicenseKeyService licenseKeyService, ILoggingService loggingService)
        {
            this.licenseKeyService = licenseKeyService;
            this.loggingService = loggingService;
        }

        [HttpPost("verify-license/{accessToken}")]
        public async Task<IActionResult> VerifyLicense([FromRoute] string accessToken, [FromBody] VerifyLicenseRequest model)
        {
            // testing getting Mac address

            //Console.WriteLine("My Mac Address: " + licenseKeyService.GetMacAddress());

            try
            {
                await licenseKeyService.VerifyLicense(model, accessToken);
                await loggingService.AddActivationLog(model.LicenseKey, true, model.FigmaUserId, "User with Figma Id: <FigmaId> at <time> successfully verified license with License Key: <LicenseKey>");
                return Ok();
            }
            catch (Exception ex)
            {
                var msg = $"User with Figma Id: <FigmaId> at <time> did not successfully verified license with License Key: <LicenseKey> because of the problem: \"{ex.Message}\"";
                await loggingService.AddActivationLog(model.LicenseKey, false, model.FigmaUserId, msg);
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
