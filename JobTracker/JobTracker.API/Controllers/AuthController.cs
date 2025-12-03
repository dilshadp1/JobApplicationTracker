
using JobTracker.Application.Command.AuthenticationCommands.CreateRefreshToken;
using JobTracker.Application.Command.AuthenticationCommands.LoginCommand;
using JobTracker.Application.Command.AuthenticationCommands.LogoutCommand;
using JobTracker.Application.Command.UserCommands.CreateUser;
using JobTracker.Application.DTO;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(LoginCommand command)
        {
            AuthResponse response = await _mediator.Send(command);
            SetRefreshTokenCookie(response.RefreshToken);
            return Ok(response);
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<AuthResponse>> RefreshToken()
        {
            string? refreshToken = Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(refreshToken))
            {
                return Unauthorized("No refresh token found.");
            }
            CreateRefreshTokenCommand command = new CreateRefreshTokenCommand { Token = refreshToken };

            AuthResponse response = await _mediator.Send(command);
            SetRefreshTokenCookie(response.RefreshToken);
            return Ok(response);

        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            string? refreshToken = Request.Cookies["refreshToken"];

            if (!string.IsNullOrEmpty(refreshToken))
            {
                LogoutCommand command = new LogoutCommand { Token = refreshToken };
                await _mediator.Send(command);
            }

            CookieOptions cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(-1) 
            };

            Response.Cookies.Append("refreshToken", "", cookieOptions);

            return Ok(new { Message = "Logged out successfully" });
        }

        private void SetRefreshTokenCookie(string refreshToken)
        {
            CookieOptions cookieOptions = new CookieOptions
            {
                HttpOnly = true,  
                Secure = true,
                Path = "/",
                SameSite = SameSiteMode.None, 
                Expires = DateTime.UtcNow.AddDays(7) 
            };

            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }
    }
}
