using MediatR;

namespace JobTracker.Application.Command.OfferCommands.DeleteOffer
{
    public class DeleteOfferCommand : IRequest<int>
    {
        public int Id { get; set; }
        public int UserId { get; set; }
    }
}