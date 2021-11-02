using net_core_backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using net_core_backend.Services;
using net_core_backend.ViewModel;
using net_core_backend.Services.Interfaces;

namespace net_core_backend.Controllers
{
    /// <summary>
    /// Example controller
    /// </summary>
    /// 
    [ApiController]
    [Route("api/[controller]")]
    public class ExampleController : ControllerBase
    {
        private readonly ILogger<ExampleController> _logger;
        private readonly IExampleService context;

        public ExampleController(ILogger<ExampleController> logger, IExampleService _context)
        {
            _logger = logger;
            context = _context;
        }


        [HttpGet("{word}")]
        public IActionResult AddSomething([FromRoute] string word)
        {
            try
            {
                var result = context.DoSomething();

                return Ok($"{result} + {word}");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("testdb")]
        public async Task<IActionResult> TestDatabase()
        {
            try
            {
                var result = await context.TestDatabase();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("protected")]
        public IActionResult TestProtected()
        {
            try
            {
                return Ok("This is a protected method");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
