using MediatR;

namespace JobTracker.Application.Command.JobApplicationCommands.DeleteJobApplication
{
    public class DeleteJobApplicationCommand : IRequest<int>
    {
        public int UserId { get; set; }
        public int JobId { get; set; }
    }
}
