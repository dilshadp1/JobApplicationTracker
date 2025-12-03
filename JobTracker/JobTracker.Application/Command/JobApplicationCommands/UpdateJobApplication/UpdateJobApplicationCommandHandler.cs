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

            if (job == null) throw new UnauthorizedAccessException("Not your Job Application to Edit");


            bool isDead = job.Status == ApplicationStatus.Rejected || job.Status == ApplicationStatus.Declined;
            if (isDead && job.Status != command.Status)
            {
                throw new InvalidOperationException($"This application is closed ({job.Status}). You cannot change the status, but you can update notes/details.");
            }

            ApplicationStatus finalStatus = command.Status;

            if (command.Status == ApplicationStatus.Interviewing && job.Interviews.Count == 0)
            {
                finalStatus = ApplicationStatus.Applied;
            }


            if (command.Status == ApplicationStatus.OfferReceived && job.Offer == null)
            {

                finalStatus = job.Interviews.Count > 0
                    ? ApplicationStatus.Interviewing
                    : ApplicationStatus.Applied;
            }

            job.UpdateJobApplication(
                command.Company,
                command.Position,
                finalStatus, 
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