using JobTracker.Application.Command.CreateJobApplication;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace JobTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobApplicationsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public JobApplicationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create(CreateJobApplicationCommand command)
        {
            int response = await _mediator.Send(command);
            return Ok(response);
        }
    }
}
