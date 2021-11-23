using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using net_core_backend.Models;
using net_core_backend.Repository;
using net_core_backend.Services.Interfaces;
using System;
using System.Threading.Tasks;


namespace net_core_backend.Controllers
{
   /* [Authorize]*/
    [Route("api/[controller]")]
    [ApiController]
    public class LicenseController : ControllerBase
    {
        private readonly ILicenseKeyService licenseKeyService;
        private readonly ILoggingService loggingService;
        private readonly IAccessTokenService accessTokenService;
        private readonly IPaginationService paginationService;

        public LicenseController(ILicenseKeyService licenseKeyService, ILoggingService loggingService, IAccessTokenService accessTokenService, IPaginationService PaginationService)
        {
            this.licenseKeyService = licenseKeyService;
            this.loggingService = loggingService;
            this.accessTokenService = accessTokenService;
            this.paginationService = PaginationService;
        }
        
        [HttpPost("get-page")]
        public async Task<IActionResult> GetPaginatedLicenses([FromBody] PaginationLicenseRequest pagingParameters)
        {
            try
            {
                var pagination = await paginationService.GetLicenses(pagingParameters);

                return Ok(pagination);
            }
            catch(Exception ex)
            {
               return  BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAllLicenses([FromRoute] int id)
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

        [AllowAnonymous]
        [HttpPost("verify-license/{accessToken}")]
        public async Task<IActionResult> VerifyLicense([FromRoute] string accessToken, [FromBody] VerifyLicenseRequest model)
        {
            try
            {
                await accessTokenService.CheckAccessToken(accessToken);
                await licenseKeyService.VerifyLicense(model);
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
