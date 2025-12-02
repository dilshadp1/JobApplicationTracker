using JobTracker.Application.Interfaces;
using JobTracker.Domain.Entities;
using MediatR;

namespace JobTracker.Application.Command.OfferCommands.UpdateOffer
{
    public class UpdateOfferCommandHandler(IOfferRepository offerRepository) : IRequestHandler<UpdateOfferCommand, int>
    {
        public async Task<int> Handle(UpdateOfferCommand command, CancellationToken cancellationToken)
        {
            Offer? offer = await offerRepository.GetOfferByIdWithJobAsync(command.Id);

            if (offer == null)
            {
                throw new KeyNotFoundException($"Offer with ID {command.Id} not found.");
            }

            if (offer.JobApplication.UserId != command.UserId)
            {
                throw new UnauthorizedAccessException("You cannot edit an offer for a job application you do not own.");
            }

            offer.Update(
                command.Salary,
                command.OfferDate,
                command.Deadline,
                command.Benefits
            );

            await offerRepository.UpdateAsync(offer);
            return offer.Id;
        }
    }
}