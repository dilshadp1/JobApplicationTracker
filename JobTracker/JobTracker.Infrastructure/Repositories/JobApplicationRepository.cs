using JobTracker.Application.DTO;
using JobTracker.Application.Interfaces;
using JobTracker.Domain.Entities;
using JobTracker.Domain.Enums;
using JobTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace JobTracker.Infrastructure.Repositories
{
    public class JobApplicationRepository(ApplicationDbContext context) : GenericRepository<JobApplication>(context), IJobApplicationRepository
    {
        public async Task<List<JobApplication>> GetJobApplicationsWithDetailsAsync(int userId)
        {
            return await _context.JobApplications.Where(j => j.UserId == userId)
                .Include(o => o.Offer)
                .Include(i => i.Interviews)
                .ToListAsync();
        }

        public async Task<JobApplication?> GetJobApplicationByIdWithDetailsAsync(int jobId, int userId)
        {
            return await context.JobApplications.Where(j => j.UserId == userId && j.Id == jobId)
                .Include(o => o.Offer)
                .Include(i => i.Interviews)
                .FirstOrDefaultAsync();
        }

        public async Task<Dictionary<ApplicationStatus, int>> GetJobStatsByUserIdAsync(int userId)
        {
            List<JobStatDto>  stats = await _context.JobApplications
                .Where(j => j.UserId == userId)
                .GroupBy(j => j.Status)
                .Select(g => new JobStatDto 
                {
                    Status = g.Key,
                    Count = g.Count()
                })
                .ToListAsync();

            return stats.ToDictionary(k => k.Status, v => v.Count);
        }

        public async Task<List<JobApplication>> GetRecentApplicationsAsync(int userId, int count)
        {
            return await _context.JobApplications
                .AsNoTracking()
                .Where(j => j.UserId == userId)
                .OrderByDescending(j => j.UpdatedAt)
                .Take(count)
                .ToListAsync();
        }


        public async Task<bool> IsJobOwnedByUserAsync(int jobId, int userId)
        {
            // This runs a "SELECT 1" or "EXISTS" query, not a "SELECT *"
            return await context.JobApplications
                .AnyAsync(j => j.Id == jobId && j.UserId == userId);
        }
        public async Task<JobApplication?> GetJobWithOfferAsync(int jobId)
        {
            return await context.JobApplications
                .Include(j => j.Offer) // Join with Offer table
                .FirstOrDefaultAsync(j => j.Id == jobId);
        }

        public async Task<bool> JobExistsAsync(int userId, string company, string position, DateTime appliedDate)
        {
            // We use .Date to ignore the time component (9:00 AM vs 10:00 AM)
            return await context.JobApplications
                .AnyAsync(j => j.UserId == userId
                            && j.Company.ToLower() == company.ToLower()
                            && j.Position.ToLower() == position.ToLower()
                            && j.AppliedDate.Date == appliedDate.Date);
        }

    }
}
