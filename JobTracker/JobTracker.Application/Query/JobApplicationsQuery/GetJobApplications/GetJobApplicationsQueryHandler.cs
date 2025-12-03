
using JobTracker.Application.DTO;
using JobTracker.Application.Interfaces;
using JobTracker.Domain.Entities;
using MediatR;


namespace JobTracker.Application.Query.JobApplicationsQuery.GetJobApplications
{
    namespace JobTracker.Application.Query.JobApplicationsQuery.GetJobApplications
    {
        public class GetJobApplicationsQueryHandler(IJobApplicationRepository jobApplicationRepository)
            : IRequestHandler<GetJobApplicationsQuery, List<JobApplicationDto>>
        {
            public async Task<List<JobApplicationDto>> Handle(GetJobApplicationsQuery query, CancellationToken cancellationToken)
            {
                List<JobApplication> jobs = await jobApplicationRepository.GetJobApplicationsWithDetailsAsync(query.UserId, query.Status);

                return jobs.Select(j => new JobApplicationDto
                {
                    Id = j.Id,
                    Company = j.Company,
                    Position = j.Position,
                    AppliedDate = j.AppliedDate,
                    CurrentStatus = j.Status,
                    JobUrl = j.JobUrl,
                    SalaryExpectation = j.SalaryExpectation,
                    Notes = j.Notes,
                    InterviewCount = j.Interviews.Count,
                    OfferStatus = j.Offer == null ? "Pending" : "Received",
                }).ToList();
            }
        }
    }
}
