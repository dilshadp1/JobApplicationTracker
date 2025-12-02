using JobTracker.Application.Interfaces;
using JobTracker.Domain.Entities;
using MediatR;

namespace JobTracker.Application.Command.InterviewCommands.CreateInterview
{
    public class CreateInterviewCommandHandler(IGenericRepository<Interview> interviewRepository, IJobApplicationRepository jobRepository) : IRequestHandler<CreateInterviewCommand, int>
    {
        public async Task<int> Handle(CreateInterviewCommand request, CancellationToken cancellationToken)
        {
            bool isUserOwner = await jobRepository.IsJobOwnedByUserAsync(request.ApplicationId, request.UserId);

            if (!isUserOwner)
            {
                throw new UnauthorizedAccessException("Not your Job Application to Add an Interview to");
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
            return newInterview.Id;
        }
    }
}
