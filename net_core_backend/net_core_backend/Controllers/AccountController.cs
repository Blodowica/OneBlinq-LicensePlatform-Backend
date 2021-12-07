using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using net_core_backend.Models;
using net_core_backend.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace net_core_backend.Controllers
{

    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService accountService;

        public AccountController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AddUserRequest model)
        {
            try
            {
                var response = await accountService.Register(model, ipAddress());

                if (response == null)
                    return BadRequest(new { message = "Validation failed." });

                setTokenCookie(response.RefreshToken);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            try
            {
                var response = await accountService.Login(model, ipAddress());

                if (response == null)
                    return BadRequest(new { message = "Validation failed." });

                setTokenCookie(response.RefreshToken);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            try
            {
                var refreshToken = Request.Cookies["refreshToken"];
                var response = await accountService.RefreshToken(refreshToken, ipAddress());

                if (response == null)
                    return Unauthorized(new { message = "Invalid token" });

                setTokenCookie(response.RefreshToken);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("revoke-token")]
        public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenRequest model = null)
        {
            // accept token from request body or cookie
            var token = model.Token ?? Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(token))
                return BadRequest(new { message = "Token is required" });

            var response = await accountService.RevokeToken(token, ipAddress());

            if (!response)
                return NotFound(new { message = "Token not found" });

            return Ok(new { message = "Token revoked" });
        }


        [AllowAnonymous]
        [HttpPost("revoke-cookie")]
        public async Task<IActionResult> RevokeCookie()
        {
            try
            {
                var refreshToken = Request.Cookies["refreshToken"];
                if (refreshToken == null)
                {
                    return BadRequest(new { message = "There wasn't a refresh token cookie in the request" });
                }

                var status = await accountService.RevokeCookie(refreshToken, ipAddress());

                if (status)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest(new { message = "The cookie is already inactive" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        private void setTokenCookie(string token)
        {
            // Run application in https mode only (self-sign for localhost)
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7),
                SameSite = SameSiteMode.None,
                Secure = true,
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }

        private string ipAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
        
        [Authorize(Roles ="Admin")]
        [HttpPost("create-admin")]
        public async Task<IActionResult> CreateAdmin([FromBody] AddUserRequest model)
        {
            try
            {
                 await accountService.CreateAdmin(model);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpGet("get-user-info")]
        public async Task<IActionResult> GetUserInfo()
        {
            try
            {
                var userInfo = await accountService.GetUserInfoDetails();

                return Ok(userInfo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost("change-user-password")]
        public async Task<IActionResult> ChangeUserPassword([FromBody] ChangePasswordRequest model)
        {
            try
            {
                await accountService.ChangePassword(model);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpPost("change-user-info")]
        public async Task<IActionResult> ChangeUserInfo([FromBody] EditUserInfoModel model)
        {
            try
            {
                await accountService.ChangeUserInfoDetails(model);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpGet("is-user-admin")]
        public async Task<IActionResult> IsUserAdmin()
        {
            try
            {
                bool admin = await accountService.IsUserAdmin();

                return Ok(admin);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpGet("get-notification-decisions")]
        public async Task<IActionResult> GetUserNotifications()
        {
            try
            {
                var response = await accountService.GetUserNotifications();

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpPost("set-notification-decisions")]
        public async Task<IActionResult> SetUserNotifications([FromBody] UserNotificationsRequest model)
        {
            try
            {
                await accountService.SetUserNotifications(model);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
