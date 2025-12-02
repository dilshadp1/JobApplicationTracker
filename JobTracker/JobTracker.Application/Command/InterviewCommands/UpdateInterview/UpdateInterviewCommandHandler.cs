using JobTracker.Application.Interfaces;
using JobTracker.Domain.Entities;
using MediatR;

namespace JobTracker.Application.Command.InterviewCommands.UpdateInterview
{
    public class UpdateInterviewCommandHandler(
        IInterviewRepository interviewRepository)
        : IRequestHandler<UpdateInterviewCommand, int>
    {
        public async Task<int> Handle(UpdateInterviewCommand command, CancellationToken cancellationToken)
        {
            Interview? interview = await interviewRepository.GetInterviewByIdWithJobAsync(command.Id);

            if (interview == null)
            {
                throw new KeyNotFoundException($"Interview with ID {command.Id} not found.");
            }

            if (interview.JobApplication.UserId != command.UserId)
            {
                throw new UnauthorizedAccessException("Not your interview to edit.");
            }

            interview.Update(
                command.InterviewDate,
                command.RoundName,
                command.Mode,
                command.Status,
                command.LocationUrl,
                command.Feedback
            );

            await interviewRepository.UpdateAsync(interview);
            return interview.Id;
        }
    }
}