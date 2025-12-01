using JobTracker.Domain.Enums;
using MediatR;

namespace JobTracker.Application.Command.JobApplicationCommands.CreateJobApplication
{
    public class CreateJobApplicationCommand : IRequest<int>
    {
        public string Company { get; set; }
        public string Position { get; set; }
        public ApplicationStatus Status { get; set; }
        public DateTime AppliedDate { get; set; }
        public string? JobUrl { get; set; }
        public decimal? SalaryExpectation { get; set; }
        public string? Notes { get; set; }
    }
}
