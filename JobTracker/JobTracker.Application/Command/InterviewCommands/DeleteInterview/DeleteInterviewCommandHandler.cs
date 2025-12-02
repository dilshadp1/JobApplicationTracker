using JobTracker.Application.Interfaces;
using JobTracker.Domain.Entities;
using MediatR;

namespace JobTracker.Application.Command.InterviewCommands.DeleteInterview
{
    public class DeleteInterviewCommandHandler(
        IGenericRepository<Interview> interviewRepository,
         IJobApplicationRepository jobRepository)
        : IRequestHandler<DeleteInterviewCommand, int>
    {
        public async Task<int> Handle(DeleteInterviewCommand command, CancellationToken cancellationToken)
        {
            Interview? interview = await interviewRepository.GetByIdAsync(command.Id);

            if (interview == null)
            {
                throw new KeyNotFoundException($"Interview with ID {command.Id} not found.");
            }

            bool isUserOwner = await jobRepository.IsJobOwnedByUserAsync(interview.ApplicationId, command.UserId);

            if (!isUserOwner)
            {
                throw new UnauthorizedAccessException("Not your Job Application to Add an Interview to");
            }

            await interviewRepository.DeleteAsync(interview);
            return interview.Id;
        }
    }
}