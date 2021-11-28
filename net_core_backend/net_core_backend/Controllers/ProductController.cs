using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using net_core_backend.Models;
using net_core_backend.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService productService;
   

        public ProductController(IProductService productService)
        {
            this.productService = productService;
          
        }
        
        [HttpPost("toggle-product/{productId}")]
        public async Task<IActionResult> ToggleProduct([FromRoute] string productId)
        {
            try
            {
                await productService.ToggleProduct(Convert.ToInt32(productId));

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("{productId}/{maxUses}")]
        public async Task<IActionResult> EditMaxUses([FromRoute] string productId, string maxUses)
        {
            try
            {
                await productService.EditMaxUses(Convert.ToInt32(productId), Convert.ToInt32(maxUses));

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        //will fetch product data from gumroad and update it with the newest data
        [HttpPost("refresh-products")]
        public async Task<IActionResult> RefreshProduct()
        {
            try
            {
                await productService.RefreshProduct();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
