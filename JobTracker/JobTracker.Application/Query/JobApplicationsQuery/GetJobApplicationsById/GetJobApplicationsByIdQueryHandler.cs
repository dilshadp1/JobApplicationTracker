using JobTracker.Application.DTO;
using JobTracker.Application.Interfaces;
using JobTracker.Domain.Entities;
using MediatR;

namespace JobTracker.Application.Query.JobApplicationsQuery.GetJobApplicationById
{
    public class GetJobApplicationByIdQueryHandler(IJobApplicationRepository jobRepository)
        : IRequestHandler<GetJobApplicationByIdQuery, JobApplicationDto>
    {
        public async Task<JobApplicationDto> Handle(GetJobApplicationByIdQuery query, CancellationToken cancellationToken)
        {
            JobApplication? job = await jobRepository.GetJobApplicationByIdWithDetailsAsync(query.Id, query.UserId);
            if (job == null)
            {
                throw new KeyNotFoundException($"Job Application with ID {query.Id} not found.");
            }
            return new JobApplicationDto
            {
                Id = job.Id,
                Company = job.Company,
                Position = job.Position,
                AppliedDate = job.AppliedDate,
                CurrentStatus = job.Status,
                JobUrl = job.JobUrl,
                SalaryExpectation = job.SalaryExpectation,
                Notes = job.Notes,
                InterviewCount = job.Interviews?.Count ?? 0,
                OfferStatus = job.Offer == null ? "Pending" : "Received"
            };
        }
    }
}