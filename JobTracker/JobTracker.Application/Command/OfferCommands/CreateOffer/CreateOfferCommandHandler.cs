using JobTracker.Application.Interfaces;
using JobTracker.Domain.Entities;
using MediatR;


namespace JobTracker.Application.Command.OfferCommands.CreateOffer
{
    public class CreateOfferCommandHandler(IGenericRepository<Offer> offerRepository, IJobApplicationRepository jobRepository) : IRequestHandler<CreateOfferCommand, int>
    {
        public async Task<int> Handle(CreateOfferCommand request, CancellationToken cancellationToken)
        {
            var job = await jobRepository.GetJobWithOfferAsync(request.ApplicationId);

            if (job == null) throw new KeyNotFoundException("Job not found.");

            if (job.UserId != request.UserId)
                throw new UnauthorizedAccessException("Not your job.");

            if (job.Offer != null)
                throw new InvalidOperationException("Offer already exists.");

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
