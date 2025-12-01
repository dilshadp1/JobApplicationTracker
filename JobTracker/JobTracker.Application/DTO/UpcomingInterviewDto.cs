using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Application.DTO
{
    public class UpcomingInterviewDto
    {
        public int InterviewId { get; set; }
        public string Company { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string RoundName { get; set; } = string.Empty; 
        public string Mode { get; set; } = string.Empty; 
    }
}
