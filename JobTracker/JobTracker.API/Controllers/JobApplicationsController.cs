using JobTracker.Application.Command.JobApplicationCommands.CreateJobApplication;
using JobTracker.Application.DTO;
using JobTracker.Application.Query.JobApplicationsQuery.GetJobApplications;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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

        [HttpGet]
        public async Task<ActionResult<List<JobApplicationDto>>> GetAll()
        {
            GetJobApplicationsQuery query = new GetJobApplicationsQuery { UserId = 1 };
            List<JobApplicationDto> response = await _mediator.Send(query);
            return Ok(response);
        }
    }
}
