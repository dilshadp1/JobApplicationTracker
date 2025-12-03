using JobTracker.Application.Interfaces;
using JobTracker.Domain.Entities;
using JobTracker.Domain.Enums;
using MediatR;

namespace JobTracker.Application.Command.InterviewCommands.UpdateInterview
{
    public class UpdateInterviewCommandHandler(
        IInterviewRepository interviewRepository,
        IJobApplicationRepository jobRepository)
        : IRequestHandler<UpdateInterviewCommand, int>
    {
        public async Task<int> Handle(UpdateInterviewCommand command, CancellationToken cancellationToken)
        {
            Interview? interview = await interviewRepository.GetInterviewByIdWithJobAsync(command.Id);

            if (interview == null)
                throw new KeyNotFoundException($"Interview with ID {command.Id} not found.");

            if (interview.JobApplication.UserId != command.UserId)
                throw new UnauthorizedAccessException("Not your interview to edit.");

            if (command.InterviewDate.Date < interview.JobApplication.AppliedDate.Date)
            {
                throw new InvalidOperationException($"Interview date ({command.InterviewDate.ToShortDateString()}) cannot be earlier than the application date ({interview.JobApplication.AppliedDate.ToShortDateString()}).");
            }

            if (interview.Status == InterviewStatus.Completed && command.Status != InterviewStatus.Completed)
            {
                throw new InvalidOperationException("This interview is marked as Completed and cannot be reverted.");
            }

            interview.Update(
                command.InterviewDate,
                command.RoundName,
                command.Mode,
                command.Status,
                command.LocationUrl,
                command.Feedback
            );

            bool jobStatusChanged = false;

            if (command.Status == InterviewStatus.Cancelled)
            {
                if (interview.JobApplication.Status != ApplicationStatus.Rejected)
                {
                    interview.JobApplication.UpdateStatus(ApplicationStatus.Rejected);
                    jobStatusChanged = true;
                }
            }
            else if (command.Status == InterviewStatus.NoShow)
            {
                if (interview.JobApplication.Status != ApplicationStatus.Declined)
                {
                    interview.JobApplication.UpdateStatus(ApplicationStatus.Declined);
                    jobStatusChanged = true;
                }
            }

            await interviewRepository.UpdateAsync(interview);

            if (jobStatusChanged)
            {
                await jobRepository.UpdateAsync(interview.JobApplication);
            }

            return interview.Id;
        }
    }
}