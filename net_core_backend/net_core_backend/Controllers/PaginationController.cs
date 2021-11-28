
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using net_core_backend.Models;
using net_core_backend.Models.Pagination;
using net_core_backend.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class PaginationController : ControllerBase
    {
        private readonly IPaginationService paginationService;

        public PaginationController(IPaginationService PaginationService)
        {
            
            this.paginationService = PaginationService;
        }

        [HttpPost("get-Licenses")]
        public async Task<IActionResult> GetPaginatedLicenses([FromBody] PaginationLicenseRequest pagingParameters)
        {
            try
            {
                var pagination = await paginationService.GetLicenses(pagingParameters);

                return Ok(pagination);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("get-AccessTokens")]
        public async Task<IActionResult> GetPaginatedAccessTokens([FromBody] PaginationAccessTokenRequest pagingParameters)
        {
            try
            {
                var response = await paginationService.GetAccesTokens(pagingParameters);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("get-Products")]
        public async Task<IActionResult> GetPaginatedProducts([FromBody] PaginationProductRequest pagingParameters)
        {
            try
            {
                var pagination = await paginationService.GetProducts(pagingParameters);

                return Ok(pagination);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("get-Users")]
        public async Task<IActionResult> GetPaginatedUsers([FromBody] PaginationUserRequest pagingParameters)
        {
            try
            {
                var pagination = await paginationService.GetUsers(pagingParameters);

                return Ok(pagination);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("get-FreeTrials")]
        public async Task<IActionResult> GetPaginatedFreeTrials([FromBody] PaginationFreeTrialRequest pagingParameters)
        {
            try
            {
                var pagination = await paginationService.GetFreeTrails(pagingParameters);

                return Ok(pagination);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
