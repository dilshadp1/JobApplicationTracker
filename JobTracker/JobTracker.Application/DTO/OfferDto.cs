namespace JobTracker.Application.DTO
{
    public class OfferDto
    {
        public int Id { get; set; }
        public int ApplicationId { get; set; }
        public decimal Salary { get; set; }
        public DateTime OfferDate { get; set; }
        public DateTime Deadline { get; set; }
        public string? Benefits { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public string JobPosition { get; set; } = string.Empty;
    }
}