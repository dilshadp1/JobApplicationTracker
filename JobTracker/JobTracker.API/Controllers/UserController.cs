using JobTracker.Application.Command.UserCommands.CreateUser;
using JobTracker.Application.Command.UserCommands.UpdateUser;
using JobTracker.Application.DTO;
using JobTracker.Application.Query.GetProfileQuery;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace JobTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<ActionResult<int>> Register(CreateUserCommand command)
        {
            int userId = await _mediator.Send(command);

            return Ok(new
            {
                UserId = userId,
                Message = "User registered successfully!"
            });
        }

        [HttpGet]
        public async Task<ActionResult<ProfileDto>> GetProfile()
        {
            ProfileDto profile = await _mediator.Send(new GetProfileQuery());
            return Ok(profile);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateProfile(UpdateProfileCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
