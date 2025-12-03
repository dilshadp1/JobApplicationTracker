using JobTracker.Application.Interfaces;
using JobTracker.Domain.Entities;
using MediatR;

namespace JobTracker.Application.Command.OfferCommands.UpdateOffer
{
    public class UpdateOfferCommandHandler(
        IOfferRepository offerRepository,
        IJobApplicationRepository jobRepository) 
        : IRequestHandler<UpdateOfferCommand, int>
    {
        public async Task<int> Handle(UpdateOfferCommand command, CancellationToken cancellationToken)
        {
            Offer? offer = await offerRepository.GetByIdAsync(command.Id);

            if (offer == null)
            {
                throw new KeyNotFoundException($"Offer with ID {command.Id} not found.");
            }

            JobApplication? job = await jobRepository.GetJobApplicationByIdWithDetailsAsync(offer.ApplicationId, command.UserId);

            if (job == null)
            {
                throw new UnauthorizedAccessException("You cannot edit an offer for a job application you do not own.");
            }

            if (command.OfferDate.Date < job.AppliedDate.Date)
            {
                throw new InvalidOperationException($"Offer date ({command.OfferDate.ToShortDateString()}) cannot be earlier than the application date ({job.AppliedDate.ToShortDateString()}).");
            }

            if (job.Interviews != null && job.Interviews.Count > 0)
            {
                DateTime lastInterviewDate = job.Interviews.Max(i => i.InterviewDate);

                if (command.OfferDate.Date < lastInterviewDate.Date)
                {
                    throw new InvalidOperationException($"Offer date cannot be earlier than your last interview on {lastInterviewDate.ToShortDateString()}.");
                }
            }

            if (command.Deadline.Date < command.OfferDate.Date)
            {
                throw new InvalidOperationException("The deadline to accept cannot be earlier than the offer received date.");
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