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

        public LicenseKeyController(ILicenseKeyService licenseKeyService)
        {
            this.licenseKeyService = licenseKeyService;
        }

        [HttpPost("verify-license/{accessToken}")]
        public async Task<IActionResult> VerifyLicense([FromRoute] string accessToken, [FromBody] VerifyLicenseRequest model)
        {
            try
            {
                await licenseKeyService.VerifyLicense(model, accessToken);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
