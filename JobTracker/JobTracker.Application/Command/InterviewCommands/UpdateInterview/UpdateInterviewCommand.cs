using JobTracker.Domain.Enums;
using MediatR;

namespace JobTracker.Application.Command.InterviewCommands.UpdateInterview
{
    public class UpdateInterviewCommand : IRequest<int>
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime InterviewDate { get; set; }
        public string RoundName { get; set; }
        public InterviewMode Mode { get; set; }
        public InterviewStatus Status { get; set; }
        public string? LocationUrl { get; set; }
        public string? Feedback { get; set; } 
    }
}