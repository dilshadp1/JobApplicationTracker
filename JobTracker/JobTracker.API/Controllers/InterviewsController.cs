using JobTracker.Application.Command.InterviewCommands.CreateInterview;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace JobTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InterviewsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public InterviewsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create(CreateInterviewCommand command)
        {
            int response = await _mediator.Send(command);
            return Ok(response);
        }
    }
}
