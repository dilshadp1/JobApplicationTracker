using JobTracker.Application.Interfaces;
using JobTracker.Domain.Entities;
using MediatR;

namespace JobTracker.Application.Command.JobApplicationCommands.CreateJobApplication
{
    public class CreateJobApplicationCommandHandler(IGenericRepository<JobApplication> jobApplicationRepository) : IRequestHandler<CreateJobApplicationCommand, int>
    {
        public async Task<int> Handle(CreateJobApplicationCommand request, CancellationToken cancellationToken)
        {
            JobApplication newJobApplication = new JobApplication(
                1,
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
