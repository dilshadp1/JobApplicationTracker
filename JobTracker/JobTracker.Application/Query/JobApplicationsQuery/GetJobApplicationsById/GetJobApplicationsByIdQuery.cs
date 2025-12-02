using JobTracker.Application.DTO;
using MediatR;

namespace JobTracker.Application.Query.JobApplicationsQuery.GetJobApplicationById
{
    public class GetJobApplicationByIdQuery : IRequest<JobApplicationDto>
    {
        public int Id { get; set; }
        public int UserId { get; set; }  
    }
}