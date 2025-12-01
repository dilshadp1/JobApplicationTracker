using JobTracker.Domain.Enums;

namespace JobTracker.Application.DTO
{
    public class JobApplicationDto
    {
        public string Company { get; set; }
        public string Position { get; set; }
        public DateTime AppliedDate { get; set; }
        public ApplicationStatus CurrentStatus { get; set; }
        public string? JobUrl { get; set; }
        public decimal? SalaryExpectation { get; set; }
        public string? Notes { get; set; }

        public int? InterviewCount { get; set; }
        public string? OfferStatus { get; set; }

}
}
