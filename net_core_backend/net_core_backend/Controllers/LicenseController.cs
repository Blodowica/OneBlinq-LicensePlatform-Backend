using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using net_core_backend.Models;
using net_core_backend.Services.Interfaces;
using System;
using System.Threading.Tasks;


namespace net_core_backend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LicenseController : ControllerBase
    {
        private readonly ILicenseKeyService licenseKeyService;
        private readonly ILoggingService loggingService;
        private readonly IAccessTokenService accessTokenService;


        public LicenseController(ILicenseKeyService licenseKeyService, ILoggingService loggingService, IAccessTokenService accessTokenService)
        {
            this.licenseKeyService = licenseKeyService;
            this.loggingService = loggingService;
            this.accessTokenService = accessTokenService;
       
        }
        

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLicense([FromRoute] int id)
        {
            try
            {
                var license = await licenseKeyService.GetLicenseDetails(id);

                if (license == null)
                    return BadRequest($"License with ID: {id} not found");

                return Ok(license);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpGet("user-license/{userId}")]
        public async Task<IActionResult> GetUserLicenses([FromRoute] string userId)
        {
            try
            {
                var licenses = await licenseKeyService.GetAllUserLicenses(Convert.ToInt32(userId));

                return Ok(licenses);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("toggle-license/{licenseId}")]
        public async Task<IActionResult> ToggleLicenseState([FromRoute] string licenseId)
        {
            try
            {
                await licenseKeyService.toggleLicenseState(Convert.ToInt32(licenseId));

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpPost("verify-license/{accessToken}")]
        public async Task<IActionResult> VerifyLicense([FromRoute] string accessToken, [FromBody] VerifyLicenseRequest model)
        {
            try
            {
                await accessTokenService.CheckAccessToken(accessToken);
                await licenseKeyService.VerifyLicense(model);
                await loggingService.AddActivationLog(model.LicenseKey, true, model.UniqueUserId, model.PlatformName, $"External ID: \"{model.UniqueUserId}\" at {DateTime.UtcNow} on platform {model.PlatformName} successfully verified license with License Key: \"{model.LicenseKey}\"");
                return Ok();
            }
            catch (Exception ex)
            {
                var msg = $"External ID: \"{model.UniqueUserId}\" at {DateTime.UtcNow} on platform {model.PlatformName} failed verification using License Key: \"{model.LicenseKey}\"\nReason: \"{ex.Message}\"";
                await loggingService.AddActivationLog(model.LicenseKey, false, model.UniqueUserId, model.PlatformName, msg);
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
