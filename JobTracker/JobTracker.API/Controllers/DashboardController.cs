using JobTracker.Application.DTO;
using JobTracker.Application.Interfaces;
using JobTracker.Application.Query.GetDashboardStatsQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class DashboardController : ControllerBase
    {
        private readonly IMediator _mediator;
        
        public DashboardController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<DashboardStatsDto>> GetStats()
        {
            DashboardStatsDto stats = await _mediator.Send(new GetDashboardStatsQuery());
            return Ok(stats);
        }
    }
}
