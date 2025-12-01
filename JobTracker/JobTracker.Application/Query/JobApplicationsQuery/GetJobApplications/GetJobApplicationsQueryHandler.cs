
using JobTracker.Application.DTO;
using JobTracker.Application.Interfaces;
using JobTracker.Domain.Entities;
using MediatR;


namespace JobTracker.Application.Query.JobApplicationsQuery.GetJobApplications
{
    public class GetJobApplicationsQueryHandler : IRequestHandler<GetJobApplicationsQuery, List<JobApplicationDto>>
    {
        private readonly IJobApplicationRepository _jobApplicationRepository;
        public GetJobApplicationsQueryHandler(IJobApplicationRepository jobApplicationRepository)
        {
            _jobApplicationRepository = jobApplicationRepository;
        }
        public async Task<List<JobApplicationDto>> Handle(GetJobApplicationsQuery query, CancellationToken cancellationToken)
        {
            List<JobApplication> jobs = await _jobApplicationRepository.GetApplicationsWithDetailsAsync(query.UserId);
            return jobs.Select(j => new JobApplicationDto
            {
                Company = j.Company,
                Position = j.Position,
                AppliedDate = j.AppliedDate,
                CurrentStatus = j.Status,
                JobUrl = j.JobUrl,
                SalaryExpectation = j.SalaryExpectation,
                Notes = j.Notes,

                InterviewCount = j.Interviews.Count,
                
                OfferStatus= j.Offer==null ? "Pending" : "Received",

            }).ToList();
        }
    }
}
