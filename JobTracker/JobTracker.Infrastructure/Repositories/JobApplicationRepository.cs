using JobTracker.Application.Interfaces;
using JobTracker.Domain.Entities;
using JobTracker.Domain.Enums;
using JobTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace JobTracker.Infrastructure.Repositories
{
    public class JobApplicationRepository(ApplicationDbContext context) : GenericRepository<JobApplication>(context), IJobApplicationRepository
    {
        public async Task<List<JobApplication>> GetApplicationsWithDetailsAsync(int userId)
        {
            return await context.JobApplications.Where(j => j.UserId == userId)
                .Include(o => o.Offer)
                .Include(i => i.Interviews)
                .ToListAsync();
        }

        public async Task<Dictionary<ApplicationStatus, int>> GetJobStatsByUserIdAsync(int userId)
        {
            var stats = await context.Set<JobApplication>()
                .Where(j => j.UserId == userId)
                .GroupBy(j => j.Status)
                .Select(g => new
                {
                    Status = g.Key,
                    Count = g.Count()
                })
                .ToListAsync();

            return stats.ToDictionary(k => k.Status, v => v.Count);
        }

       
    }
}
