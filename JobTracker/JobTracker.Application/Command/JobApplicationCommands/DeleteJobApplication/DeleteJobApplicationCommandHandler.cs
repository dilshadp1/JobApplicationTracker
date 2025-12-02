using JobTracker.Application.Interfaces;
using JobTracker.Domain.Entities;
using MediatR;


namespace JobTracker.Application.Command.JobApplicationCommands.DeleteJobApplication
{
    public class DeleteJobApplicationCommandHandler(IGenericRepository<JobApplication> jobApplicationRepository) : IRequestHandler<DeleteJobApplicationCommand,int>
    {
        public async Task<int> Handle(DeleteJobApplicationCommand command, CancellationToken cancellationToken)
        {

            JobApplication? jobApplication = await jobApplicationRepository.GetByIdAsync(command.JobId);

            if (jobApplication == null || jobApplication.UserId != command.UserId)
            {
                throw new UnauthorizedAccessException("Not your Job Application to Delete");
            }

            await jobApplicationRepository.DeleteAsync(jobApplication);
            return jobApplication.Id;
            
        }
    }
}
