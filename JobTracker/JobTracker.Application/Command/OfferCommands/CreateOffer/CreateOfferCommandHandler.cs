using JobTracker.Application.Interfaces;
using JobTracker.Domain.Entities;
using JobTracker.Domain.Enums;
using MediatR;


namespace JobTracker.Application.Command.OfferCommands.CreateOffer
{
    public class CreateOfferCommandHandler(IGenericRepository<Offer> offerRepository, IJobApplicationRepository jobRepository) : IRequestHandler<CreateOfferCommand, int>
    {
        public async Task<int> Handle(CreateOfferCommand request, CancellationToken cancellationToken)
        {
            JobApplication? job = await jobRepository.GetJobApplicationByIdWithDetailsAsync(request.ApplicationId, request.UserId);
            if (job == null) throw new KeyNotFoundException("Job application not found.");

            if (job.UserId != request.UserId)
                throw new UnauthorizedAccessException("Not your job.");

            if (job.Offer != null)
                throw new InvalidOperationException("Offer already exists.");

            if (request.OfferDate.Date < job.AppliedDate.Date)
            {
                throw new InvalidOperationException($"Offer date cannot be earlier than the application date ({job.AppliedDate.ToShortDateString()}).");
            }

            if (job.Interviews != null && job.Interviews.Any())
            {
                DateTime lastInterviewDate = job.Interviews.Max(i => i.InterviewDate);

                if (request.OfferDate.Date < lastInterviewDate.Date)
                {
                    throw new InvalidOperationException($"Offer date cannot be earlier than your last interview on {lastInterviewDate.ToShortDateString()}.");
                }
            }

            if (job.Status == ApplicationStatus.Hired ||
                job.Status == ApplicationStatus.Rejected ||
                job.Status == ApplicationStatus.Declined)
            {
                throw new InvalidOperationException($"Cannot add an offer to a closed job ({job.Status}). Re-open the job first.");
            }

            if (job.Status != ApplicationStatus.OfferReceived)
            {
                job.UpdateStatus(ApplicationStatus.OfferReceived);
            }

            Offer newOffer = new Offer(
                request.ApplicationId,
                request.Salary,
                request.OfferDate,
                request.Deadline,
                request.Benefits
            );
            await offerRepository.AddAsync(newOffer);
            await jobRepository.UpdateAsync(job);
            return newOffer.Id;
        }
    }
}
