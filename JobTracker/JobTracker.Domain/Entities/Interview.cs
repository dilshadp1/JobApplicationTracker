using JobTracker.Domain.Enums;


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
        public Interview(int applicationId,DateTime interviewDate,string roundName, InterviewMode mode,string? locationUrl) { 

            ApplicationId = applicationId;
            InterviewDate= interviewDate;
            RoundName = roundName;
            Status = InterviewStatus.Scheduled;
            Mode = mode;
            LocationUrl = locationUrl;
            
        }

        public void UpdateStatus(InterviewStatus status)
        {
            Status = status;
        }
        public void MarkAsCompleted(string feedback)
        {
            Status = InterviewStatus.Completed;
            Feedback=feedback;
        }
    }
}
