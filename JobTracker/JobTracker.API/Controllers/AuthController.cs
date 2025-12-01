
using JobTracker.Application.Command.AuthenticationCommands.CreateRefreshToken;
using JobTracker.Application.Command.AuthenticationCommands.LoginCommand;
using JobTracker.Application.Command.UserCommands.CreateUser;
using JobTracker.Application.DTO;
using MediatR;
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

        [HttpPost("register")]
        public async Task<ActionResult<int>> Register(CreateUserCommand command)
        {
            var userId = await _mediator.Send(command);

            return Ok(new
            {
                UserId = userId,
                Message = "User registered successfully!"
            });
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(LoginCommand command)
        {
            var response = await _mediator.Send(command);
            SetRefreshTokenCookie(response.RefreshToken);
            response.RefreshToken = string.Empty;
            return Ok(new 
            { 
                Token = response.AccessToken, 
                Message = "Login successful!" 
            });
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<AuthResponse>> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(refreshToken))
            {
                return Unauthorized("No refresh token found.");
            }
            var command = new CreateRefreshTokenCommand { Token = refreshToken };

            var response = await _mediator.Send(command);
            SetRefreshTokenCookie(response.RefreshToken);
            response.RefreshToken = string.Empty;
            return Ok(response);
        }

        private void SetRefreshTokenCookie(string refreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,  
                Secure = true,   
                SameSite = SameSiteMode.Strict, 
                Expires = DateTime.UtcNow.AddDays(7) 
            };

            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }
    }
}
