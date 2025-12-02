using JobTracker.Domain.Entities;

namespace JobTracker.Application.Interfaces
{
    public interface IOfferRepository : IGenericRepository<Offer>
    {
        Task<Offer?> GetOfferByIdWithJobAsync(int id);
        Task<List<Offer>> GetAllOffersByUserIdAsync(int userId);
    }
}