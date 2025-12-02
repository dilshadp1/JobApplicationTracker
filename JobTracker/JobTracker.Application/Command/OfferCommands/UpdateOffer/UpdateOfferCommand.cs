using MediatR;

namespace JobTracker.Application.Command.OfferCommands.UpdateOffer
{
    public class UpdateOfferCommand : IRequest<int>
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal Salary { get; set; }
        public DateTime OfferDate { get; set; }
        public DateTime Deadline { get; set; }
        public string? Benefits { get; set; }
    }
}