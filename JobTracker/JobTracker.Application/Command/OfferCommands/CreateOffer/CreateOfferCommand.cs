using MediatR;

namespace JobTracker.Application.Command.OfferCommands.CreateOffer
{
    public class CreateOfferCommand : IRequest<int>
    {
        public int UserId { get; set; }
        public int ApplicationId { get; set; }
        public decimal Salary { get; set; }
        public DateTime OfferDate { get; set; }
        public DateTime Deadline { get; set; }
        public string? Benefits { get; set; }
    }
}
