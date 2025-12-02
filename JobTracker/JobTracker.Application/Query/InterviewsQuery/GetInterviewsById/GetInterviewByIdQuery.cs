using JobTracker.Application.DTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Application.Query.InterviewsQuery.GetInterviewsById
{
    public class GetInterviewByIdQuery : IRequest<InterviewDto>
    {
        public int Id { get; set; }
        public int UserId { get; set; }
    }
}
