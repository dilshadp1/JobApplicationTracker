using JobTracker.Application.Interfaces;
using JobTracker.Domain.Entities;
using JobTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace JobTracker.Infrastructure.Repositories
{
    public class OfferRepository(ApplicationDbContext context) : GenericRepository<Offer>(context), IOfferRepository
    {
        public async Task<Offer?> GetOfferByIdWithJobAsync(int id)
        {
            return await context.Offers
                .Include(o => o.JobApplication) 
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<List<Offer>> GetAllOffersByUserIdAsync(int userId)
        {
            return await context.Offers
                .Include(o => o.JobApplication)
                .Where(o => o.JobApplication.UserId == userId)
                .OrderByDescending(o => o.OfferDate)
                .ToListAsync();
        }
    }
}