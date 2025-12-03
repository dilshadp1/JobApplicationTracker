using JobTracker.Application.Interfaces;
using JobTracker.Domain.Entities;
using JobTracker.Domain.Enums;
using MediatR;

namespace JobTracker.Application.Command.InterviewCommands.CreateInterview
{
    public class CreateInterviewCommandHandler(IGenericRepository<Interview> interviewRepository, IJobApplicationRepository jobRepository) : IRequestHandler<CreateInterviewCommand, int>
    {
        public async Task<int> Handle(CreateInterviewCommand request, CancellationToken cancellationToken)
        {

            JobApplication? job = await jobRepository.GetByIdAsync(request.ApplicationId);

            if (job == null || job.UserId != request.UserId)
            {
                throw new UnauthorizedAccessException("Not your Job Application.");
            }

            Interview newInterview = new Interview(
                request.ApplicationId,
                request.InterviewDate,
                request.RoundName,
                request.Mode,
                request.LocationUrl
                );

            newInterview.UpdateStatus(request.Status);
            await interviewRepository.AddAsync(newInterview);

            if (job.Status == ApplicationStatus.Applied)
            {
                job.UpdateStatus(ApplicationStatus.Interviewing);
                await jobRepository.UpdateAsync(job);
            }

            return newInterview.Id;
        }
    }
}
