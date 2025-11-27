using Azure;
using JobTracker.Application.Command.CreateUser;
using JobTracker.Application.Command.LoginCommand;
using JobTracker.Application.Command.RefreshToken;
using JobTracker.Application.DTO;
using JobTracker.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
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
            try
            {
                var userId = await _mediator.Send(command);

                return Ok(new
                {
                    UserId = userId,
                    Message = "User registered successfully!"
                });
            }
            catch (FluentValidation.ValidationException ex)
            {
                return BadRequest(new { Errors = ex.Errors.Select(e => e.ErrorMessage) });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(LoginCommand command)
        {
            try
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
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<AuthResponse>> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(refreshToken))
            {
                return Unauthorized("No refresh token found.");
            }
            var command = new RefreshTokenCommand { Token = refreshToken };

            try
            {
                var response = await _mediator.Send(command);
                SetRefreshTokenCookie(response.RefreshToken);
                response.RefreshToken = string.Empty;
                return Ok(response);
            }
            catch (Exception)
            {
                return Unauthorized("Invalid token.");
            }
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
