using JobTracker.Application.Interfaces;
using JobTracker.Domain.Entities;
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
    }
}
