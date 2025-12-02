using JobTracker.Application.DTO;
using JobTracker.Application.Interfaces;
using JobTracker.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Application.Query.InterviewsQuery.GetInterviewsById
{
    public class GetInterviewByIdQueryHandler(IInterviewRepository interviewRepository)
            : IRequestHandler<GetInterviewByIdQuery, InterviewDto>
    {
        public async Task<InterviewDto> Handle(GetInterviewByIdQuery query, CancellationToken cancellationToken)
        {
            Interview? interview = await interviewRepository.GetInterviewByIdWithJobAsync(query.Id);

            if (interview == null)
            {
                throw new KeyNotFoundException($"Interview with ID {query.Id} not found.");
            }

            if (interview.JobApplication.UserId != query.UserId)
            {
                throw new UnauthorizedAccessException("You are not authorized to view this interview.");
            }

            return new InterviewDto
            {
                Id = interview.Id,
                ApplicationId = interview.ApplicationId,
                CompanyName = interview.JobApplication.Company,
                JobPosition = interview.JobApplication.Position,
                InterviewDate = interview.InterviewDate,
                RoundName = interview.RoundName,
                Mode = interview.Mode,
                Status = interview.Status,
                LocationUrl = interview.LocationUrl,
                Feedback = interview.Feedback
            };
        }
    }
}
