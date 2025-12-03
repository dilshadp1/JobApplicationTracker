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

        public async Task<List<Offer>> GetAllOffersByUserIdAsync(int userId, string? filter, string? sort)
        {
            IQueryable<Offer> query = context.Offers
                .Include(o => o.JobApplication)
                .Where(o => o.JobApplication.UserId == userId);

            DateTime today = DateTime.UtcNow.Date;

            if (!string.IsNullOrEmpty(filter))
            {
                if (filter.ToLower() == "active")
                    query = query.Where(o => o.Deadline >= today);
                else if (filter.ToLower() == "expired")
                    query = query.Where(o => o.Deadline < today);
            }

            if (!string.IsNullOrEmpty(sort) && sort.ToLower() == "salaryhigh")
            {
                query = query.OrderByDescending(o => o.Salary);
            }
            else if (!string.IsNullOrEmpty(sort) && sort.ToLower() == "salarylow")
            {
                query = query.OrderBy(o => o.Salary);
            }
            else
            {
                query = query
                    .OrderBy(o => o.Deadline < today)
                    .ThenBy(o => o.Deadline);
            }

            return await query.ToListAsync();
        }
    }
}