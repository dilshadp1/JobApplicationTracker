using JobTracker.Application.Interfaces;
using JobTracker.Domain.Entities;
using MediatR;

namespace JobTracker.Application.Command.CreateInterview
{
    public class CreateInterviewCommandHandler(IGenericRepository<Interview> interviewRepository) : IRequestHandler<CreateInterviewCommand, int>
    {
        public async Task<int> Handle (CreateInterviewCommand request,CancellationToken cancellationToken)
        {
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
