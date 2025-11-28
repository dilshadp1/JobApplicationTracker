using JobTracker.Application.Interfaces;
using JobTracker.Domain.Entities;
using MediatR;


namespace JobTracker.Application.Command.CreateOffer
{
    public class CreateOfferCommandHandler(IGenericRepository<Offer> offerRepository) : IRequestHandler<CreateOfferCommand, int>
    {
        public async Task<int> Handle (CreateOfferCommand request,CancellationToken cancellationToken)
        {
            Offer newOffer = new Offer(
                request.ApplicationId,
                request.Salary,
                request.OfferDate,
                request.Deadline,
                request.Benefits
                );
            await offerRepository.AddAsync(newOffer);
            return newOffer.Id;
        }
    }
}
