using JobTracker.Application.Command.CreateOffer;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace JobTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OffersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OffersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create(CreateOfferCommand command)
        {
            int response = await _mediator.Send(command);
            return Ok(response);
        }
    }
}
