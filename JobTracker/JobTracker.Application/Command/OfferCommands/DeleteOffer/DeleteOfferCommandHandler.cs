using JobTracker.Application.Interfaces;
using JobTracker.Domain.Entities;
using MediatR;

namespace JobTracker.Application.Command.OfferCommands.DeleteOffer
{
    public class DeleteOfferCommandHandler(IOfferRepository offerRepository) : IRequestHandler<DeleteOfferCommand, int>
    {
        public async Task<int> Handle(DeleteOfferCommand command, CancellationToken cancellationToken)
        {
            Offer? offer = await offerRepository.GetOfferByIdWithJobAsync(command.Id);

            if (offer == null)
            {
                throw new KeyNotFoundException($"Offer with ID {command.Id} not found.");
            }

            if (offer.JobApplication.UserId != command.UserId)
            {
                throw new UnauthorizedAccessException("You cannot delete an offer that doesn't belong to you.");
            }

            await offerRepository.DeleteAsync(offer);
            return offer.Id;
        }
    }
}