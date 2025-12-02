using JobTracker.Application.Interfaces;
using JobTracker.Domain.Entities;
using MediatR;

namespace JobTracker.Application.Command.JobApplicationCommands.UpdateJobApplication
{
    public class UpdateJobApplicationCommandHandler(IGenericRepository<JobApplication> repository)
        : IRequestHandler<UpdateJobApplicationCommand, int>
    {
        public async Task<int> Handle(UpdateJobApplicationCommand command, CancellationToken cancellationToken)
        {
            JobApplication? job = await repository.GetByIdAsync(command.Id);

            if (job == null || job.UserId != command.UserId)
            {
                throw new UnauthorizedAccessException("Not your Job Application to Edit");
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

            await repository.UpdateAsync(job);
            return job.Id;
        }
    }
}