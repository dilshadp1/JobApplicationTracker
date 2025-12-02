using JobTracker.Application.DTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Application.Query.InterviewsQuery.GetInterviews
{
    public class GetInterviewsQuery : IRequest<List<InterviewDto>>
    {
        public int UserId { get; set; }
    }
}