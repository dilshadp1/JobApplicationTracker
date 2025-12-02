using JobTracker.Application.Command.OfferCommands.CreateOffer;
using JobTracker.Application.Command.OfferCommands.DeleteOffer;
using JobTracker.Application.Command.OfferCommands.UpdateOffer;
using JobTracker.Application.DTO;
using JobTracker.Application.Interfaces;
using JobTracker.Application.Query.OffersQuery.GetOffers;
using JobTracker.Application.Query.OffersQuery.GetOffersById;
using JobTracker.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace JobTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OffersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUserService;

        public OffersController(IMediator mediator, ICurrentUserService currentUserService)
        {
            _mediator = mediator;
            _currentUserService = currentUserService;
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create(CreateOfferCommand command)
        {
            command.UserId = _currentUserService.UserId;
            int response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<int>> Update(int id, UpdateOfferCommand command)
        {
            command.Id = id;
            command.UserId = _currentUserService.UserId;
            int response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<int>> Delete(int id)
        {
            DeleteOfferCommand command = new DeleteOfferCommand
            {
                Id = id,
                UserId = _currentUserService.UserId
            };
            int response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult<List<OfferDto>>> GetAll()
        {
            GetOffersQuery query = new GetOffersQuery { UserId = _currentUserService.UserId };
            return Ok(await _mediator.Send(query));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OfferDto>> GetById(int id)
        {
            GetOfferByIdQuery query = new GetOfferByIdQuery
            {
                Id = id,
                UserId = _currentUserService.UserId
            };
            return Ok(await _mediator.Send(query));
        }
    }
}
