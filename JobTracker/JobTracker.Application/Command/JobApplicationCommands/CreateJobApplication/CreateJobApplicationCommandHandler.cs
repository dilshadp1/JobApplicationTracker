using JobTracker.Application.Interfaces;
using JobTracker.Domain.Entities;
using MediatR;

namespace JobTracker.Application.Command.JobApplicationCommands.CreateJobApplication
{
    public class CreateJobApplicationCommandHandler(IJobApplicationRepository jobApplicationRepository) : IRequestHandler<CreateJobApplicationCommand, int>
    {
        public async Task<int> Handle(CreateJobApplicationCommand request, CancellationToken cancellationToken)
        {
            bool exists = await jobApplicationRepository.JobExistsAsync(
                request.UserId,
                request.Company,
                request.Position,
                request.AppliedDate 
            );

            if (exists)
            {
                throw new InvalidOperationException($"You have already recorded an application for {request.Company} ({request.Position}) on {request.AppliedDate.ToShortDateString()}.");
            }


            JobApplication newJobApplication = new JobApplication(
                request.UserId,
                request.Company,
                request.Position,
                request.AppliedDate,
                request.JobUrl,
                request.SalaryExpectation,
                request.Notes
                );

            newJobApplication.UpdateStatus(request.Status);

            await jobApplicationRepository.AddAsync(newJobApplication);

            return newJobApplication.Id;

        }
    }
}
