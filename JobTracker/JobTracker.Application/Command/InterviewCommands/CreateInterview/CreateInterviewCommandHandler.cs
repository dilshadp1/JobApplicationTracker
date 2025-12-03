using JobTracker.Application.Interfaces;
using JobTracker.Domain.Entities;
using JobTracker.Domain.Enums;
using MediatR;

namespace JobTracker.Application.Command.InterviewCommands.CreateInterview
{
    public class CreateInterviewCommandHandler(
        IInterviewRepository interviewRepository,
        IJobApplicationRepository jobRepository)
        : IRequestHandler<CreateInterviewCommand, int>
    {
        public async Task<int> Handle(CreateInterviewCommand request, CancellationToken cancellationToken)
        {
            JobApplication? job = await jobRepository.GetByIdAsync(request.ApplicationId);

            if (job == null) throw new KeyNotFoundException("Job not found.");
            if (job.UserId != request.UserId) throw new UnauthorizedAccessException("Not your Job.");

            if (job.Status == ApplicationStatus.Hired ||
                job.Status == ApplicationStatus.Rejected ||
                job.Status == ApplicationStatus.Declined)
            {
                throw new InvalidOperationException($"Cannot schedule interviews for a '{job.Status}' application.");
            }

            if (request.InterviewDate.Date < job.AppliedDate.Date)
            {
                throw new InvalidOperationException($"Interview date ({request.InterviewDate.ToShortDateString()}) cannot be earlier than the application date ({job.AppliedDate.ToShortDateString()}).");
            }

            string cleanRoundName = request.RoundName.Trim();
            bool roundExists = await interviewRepository.HasActiveInterviewForRoundAsync(request.ApplicationId, cleanRoundName);
            if (roundExists) throw new InvalidOperationException($"An active interview for '{cleanRoundName}' already exists.");

            Interview newInterview = new Interview(
                request.ApplicationId,
                request.InterviewDate,
                cleanRoundName,
                request.Mode,
                request.LocationUrl
            );
            newInterview.UpdateStatus(request.Status);
            await interviewRepository.AddAsync(newInterview);

            bool statusChanged = false;

            if (request.Status == InterviewStatus.Cancelled)
            {
                job.UpdateStatus(ApplicationStatus.Rejected);
                statusChanged = true;
            }
            else if (request.Status == InterviewStatus.NoShow)
            {
                job.UpdateStatus(ApplicationStatus.Declined);
                statusChanged = true;
            }
            else
            {
                if (job.Status == ApplicationStatus.Applied)
                {
                    job.UpdateStatus(ApplicationStatus.Interviewing);
                    statusChanged = true;
                }
            }

            if (statusChanged)
            {
                await jobRepository.UpdateAsync(job);
            }

            return newInterview.Id;
        }
    }
}