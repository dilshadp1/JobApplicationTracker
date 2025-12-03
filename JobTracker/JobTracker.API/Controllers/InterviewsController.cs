using JobTracker.Application.Command.InterviewCommands.CreateInterview;
using JobTracker.Application.Command.InterviewCommands.DeleteInterview;
using JobTracker.Application.Command.InterviewCommands.UpdateInterview;
using JobTracker.Application.DTO;
using JobTracker.Application.Interfaces;
using JobTracker.Application.Query.InterviewsQuery.GetInterviews;
using JobTracker.Application.Query.InterviewsQuery.GetInterviewsById;
using JobTracker.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace JobTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InterviewsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUserService;

        public InterviewsController(IMediator mediator, ICurrentUserService currentUserService)
        {
            _mediator = mediator;
            _currentUserService = currentUserService;
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create(CreateInterviewCommand command)
        {
            command.UserId = _currentUserService.UserId;
            int response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult<List<InterviewDto>>> GetAll([FromQuery] InterviewStatus? status)
        {
            GetInterviewsQuery query = new GetInterviewsQuery
            {
                UserId = _currentUserService.UserId,
                Status = status
            };
            return Ok(await _mediator.Send(query));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<InterviewDto>> GetById(int id)
        {
            var query = new GetInterviewByIdQuery { Id = id, UserId = _currentUserService.UserId };
            return Ok(await _mediator.Send(query));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<int>> Update(int id, UpdateInterviewCommand command)
        {
            command.Id = id;
            command.UserId = _currentUserService.UserId;
            int response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<int>> Delete(int id)
        {
            DeleteInterviewCommand command = new DeleteInterviewCommand { Id = id, UserId = _currentUserService.UserId };
            int response = await _mediator.Send(command);
            return Ok(response);
        }
    }
}
