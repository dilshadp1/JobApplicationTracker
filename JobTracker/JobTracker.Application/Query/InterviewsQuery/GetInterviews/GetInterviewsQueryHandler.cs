using JobTracker.Application.DTO;
using JobTracker.Application.Interfaces;
using JobTracker.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Application.Query.InterviewsQuery.GetInterviews
{
    public class GetInterviewsQueryHandler(IInterviewRepository interviewRepository)
          : IRequestHandler<GetInterviewsQuery, List<InterviewDto>>
    {
        public async Task<List<InterviewDto>> Handle(GetInterviewsQuery query, CancellationToken cancellationToken)
        {
            List<Interview> interviews = await interviewRepository.GetAllInterviewsByUserIdAsync(query.UserId);

            return interviews.Select(interview => new InterviewDto
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
            }).ToList();
        }
    }
}
