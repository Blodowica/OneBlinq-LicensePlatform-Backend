using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using net_core_backend.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService productService;

        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }

        //will fetch product data from gumroad and update it with the newest data
        [HttpPost("refresh-product/{productId}")]
        public async Task<IActionResult> RefreshProduct([FromRoute] int productId)
        {
            try
            {
                await productService.RefreshProduct(productId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
