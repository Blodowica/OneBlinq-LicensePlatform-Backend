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
        public async Task<IActionResult> Sale([FromRoute] string accessToken, [FromBody] GumroadSaleRequest sale)
        {
            if (accessToken != appSettings.GumroadAccessToken)
            {
                return Unauthorized("Incorrect accesstoken, request denied.");
            }

            if (sale.Resource_Name != "sale")
            {
                return BadRequest("Incorrect resource.");
            }

            try
            {
                await gumroadService.RegisterLicense(sale);
                return Ok("");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
