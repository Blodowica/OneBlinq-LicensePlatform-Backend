using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using net_core_backend.Models;
using net_core_backend.Services;
using net_core_backend.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace net_core_backend.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class UniqueUserController : ControllerBase
    {
        private readonly IUniqueUserService uniqueUserService;


        public UniqueUserController (IUniqueUserService _uniqueUserService)
        {
            uniqueUserService = _uniqueUserService;
        }
        
        [HttpGet("create-id")]
        public async Task<IActionResult> CreateUniqueId()
        {
            try
            {
                var response = await uniqueUserService.CreateId();

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
