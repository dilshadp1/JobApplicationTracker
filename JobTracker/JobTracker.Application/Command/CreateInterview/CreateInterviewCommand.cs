using JobTracker.Domain.Enums;
using MediatR;

namespace JobTracker.Application.Command.CreateInterview
{
    public class CreateInterviewCommand : IRequest<int>
    {
        public int ApplicationId { get; set; }
        public DateTime InterviewDate { get; set; }
        public string RoundName { get; set; }
        public InterviewMode Mode { get; set; }
        public InterviewStatus Status { get; set; }
        public string? LocationUrl { get; set; }
    }
}
