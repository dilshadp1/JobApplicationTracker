using JobTracker.Application.Interfaces;
using JobTracker.Domain.Entities;
using JobTracker.Domain.Enums;
using MediatR;

namespace JobTracker.Application.Command.JobApplicationCommands.UpdateJobApplication
{
    public class UpdateJobApplicationCommandHandler(IJobApplicationRepository jobRepository)
        : IRequestHandler<UpdateJobApplicationCommand, int>
    {
        public async Task<int> Handle(UpdateJobApplicationCommand command, CancellationToken cancellationToken)
        {
            JobApplication? job = await jobRepository.GetJobApplicationByIdWithDetailsAsync(command.Id, command.UserId);

            if (job == null)
            {
                throw new UnauthorizedAccessException("Not your Job Application to Edit");
            }

            if (command.Status == ApplicationStatus.Applied && job.Interviews.Count > 0)
            {
                throw new InvalidOperationException("Cannot revert to 'Applied' because interviews have already been scheduled.");
            }

            if (command.Status < ApplicationStatus.OfferReceived && job.Offer != null)
            {
                throw new InvalidOperationException("Cannot revert status. An offer has already been recorded for this application.");
            }

            if (job.Status == ApplicationStatus.Rejected || job.Status == ApplicationStatus.Declined)
            {
                throw new InvalidOperationException("This application is closed (Rejected/Declined) and cannot be modified.");
            }

            job.UpdateJobApplication(
                command.Company,
                command.Position,
                command.Status,
                command.AppliedDate,
                command.JobUrl,
                command.SalaryExpectation,
                command.Notes
            );

            await jobRepository.UpdateAsync(job);
            return job.Id;
        }
    }
}