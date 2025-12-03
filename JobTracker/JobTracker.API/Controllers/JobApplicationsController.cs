using JobTracker.Application.Command.JobApplicationCommands.CreateJobApplication;
using JobTracker.Application.Command.JobApplicationCommands.DeleteJobApplication;
using JobTracker.Application.Command.JobApplicationCommands.UpdateJobApplication;
using JobTracker.Application.DTO;
using JobTracker.Application.Interfaces;
using JobTracker.Application.Query.JobApplicationsQuery.GetJobApplicationById;
using JobTracker.Application.Query.JobApplicationsQuery.GetJobApplications;
using JobTracker.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace JobTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class JobApplicationsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUserService;
        public JobApplicationsController(IMediator mediator, ICurrentUserService currentUserService)
        {
            _mediator = mediator;
            _currentUserService = currentUserService;
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create(CreateJobApplicationCommand command)
        {
            command.UserId = _currentUserService.UserId;
            int response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult<List<JobApplicationDto>>> GetAll([FromQuery] ApplicationStatus? status)
        {
            GetJobApplicationsQuery query = new GetJobApplicationsQuery
            {
                UserId = _currentUserService.UserId,
                Status = status
            };
            List<JobApplicationDto> response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<int>> Delete(int id)
        {
            DeleteJobApplicationCommand command = new DeleteJobApplicationCommand();
            command.UserId = _currentUserService.UserId;
            command.JobId= id;
            int response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<JobApplicationDto>> GetById(int id)
        {
            GetJobApplicationByIdQuery query = new GetJobApplicationByIdQuery
            {
                Id = id,
                UserId = _currentUserService.UserId
            };
            JobApplicationDto result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<int>> Update(int id, UpdateJobApplicationCommand command)
        {
            command.Id = id;
            command.UserId = _currentUserService.UserId;
            int result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
