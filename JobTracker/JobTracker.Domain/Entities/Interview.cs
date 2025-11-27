using JobTracker.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Domain.Entities
{
    public class Interview
    {
        public int Id { get; private set; }
        public int ApplicationId { get; private set; }
        public DateTime InterviewDate { get; private set; }
        public string RoundName { get; private set; }
        public InterviewMode Mode { get; private set; }
        public InterviewStatus Status { get; private set; }
        public string? LocationUrl { get; private set; }
        public string? Feedback { get; private set; }

        public JobApplication JobApplication { get; private set; }

        private Interview() { }
        public Interview(int applicationId,DateTime interviewDate,string roundName,string? locationUrl,InterviewMode mode) { 

            ApplicationId = applicationId;
            InterviewDate= interviewDate;
            RoundName = roundName;
            Status = InterviewStatus.Scheduled;
            LocationUrl = locationUrl;
            Mode=mode;
            
        }

        public void MarkAsCompleted(string feedback)
        {
            Status = InterviewStatus.Completed;
            Feedback=feedback;
        }
    }
}
