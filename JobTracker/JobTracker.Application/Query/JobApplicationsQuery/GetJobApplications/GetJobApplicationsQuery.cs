
using JobTracker.Application.DTO;
using MediatR;
using System.Runtime.CompilerServices;

namespace JobTracker.Application.Query.JobApplicationsQuery.GetJobApplications
{
    public class GetJobApplicationsQuery() : IRequest<List<JobApplicationDto>>
    {
        public int UserId { get; set; }
    }
}
