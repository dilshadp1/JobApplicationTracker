using MediatR;

namespace JobTracker.Application.Command.InterviewCommands.DeleteInterview
{
    public class DeleteInterviewCommand : IRequest<int>
    {
        public int Id { get; set; }
        public int UserId { get; set; }
    }
}