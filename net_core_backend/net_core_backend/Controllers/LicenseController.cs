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
    public class LicenseController : ControllerBase
    {
        private readonly ILicenseKeyService licenseKeyService;
        private readonly ILoggingService loggingService;

        public LicenseController(ILicenseKeyService licenseKeyService, ILoggingService loggingService)
        {
            this.licenseKeyService = licenseKeyService;
            this.loggingService = loggingService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllLicenses()
        {
            try
            {
                var licenses = await licenseKeyService.GetAllLicenses();
                
                return Ok(licenses);
            }
            catch(Exception ex)
            {
               return  BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("verify-license/{accessToken}")]
        public async Task<IActionResult> VerifyLicense([FromRoute] string accessToken, [FromBody] VerifyLicenseRequest model)
        {
            try
            {
                await licenseKeyService.VerifyLicense(model, accessToken);
                await loggingService.AddActivationLog(model.LicenseKey, true, model.FigmaUserId, $"User with Figma Id: \"{model.FigmaUserId}\" at {DateTime.UtcNow} successfully verified license with License Key: \"{model.LicenseKey}\"");
                return Ok();
            }
            catch (Exception ex)
            {
                var msg = $"User with Figma Id: \"{model.FigmaUserId}\" at {DateTime.UtcNow} did not successfully verify license with License Key: \"{model.LicenseKey}\" because of the problem: \"{ex.Message}\"";
                await loggingService.AddActivationLog(model.LicenseKey, false, model.FigmaUserId, msg);
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
