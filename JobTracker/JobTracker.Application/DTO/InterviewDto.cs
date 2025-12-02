using JobTracker.Domain.Enums;
using System;

namespace JobTracker.Application.DTO
{
    public class InterviewDto
    {
        public int Id { get; set; }
        public int ApplicationId { get; set; } 
        public string CompanyName { get; set; } 
        public string JobPosition { get; set; }
        public DateTime InterviewDate { get; set; }
        public string RoundName { get; set; }
        public InterviewMode Mode { get; set; }
        public InterviewStatus Status { get; set; } 
        public string? LocationUrl { get; set; }
        public string? Feedback { get; set; }
    }
}