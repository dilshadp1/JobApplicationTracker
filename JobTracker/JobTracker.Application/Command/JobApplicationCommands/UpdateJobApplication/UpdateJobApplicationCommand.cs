using JobTracker.Domain.Enums;
using MediatR;
using System;

namespace JobTracker.Application.Command.JobApplicationCommands.UpdateJobApplication
{
    public class UpdateJobApplicationCommand : IRequest<int>
    {
        public int Id { get; set; } 
        public int UserId { get; set; } 

        public string Company { get; set; }
        public string Position { get; set; }
        public ApplicationStatus Status { get; set; } 
        public DateTime AppliedDate { get; set; }
        public string? JobUrl { get; set; }
        public decimal? SalaryExpectation { get; set; }
        public string? Notes { get; set; }
    }
}