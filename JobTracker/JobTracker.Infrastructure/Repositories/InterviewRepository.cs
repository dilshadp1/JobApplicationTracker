using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobTracker.Application.Interfaces;
using JobTracker.Domain.Entities;
using JobTracker.Domain.Enums;
using JobTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace JobTracker.Infrastructure.Repositories
{
    public class InterviewRepository(ApplicationDbContext context): GenericRepository<Interview>(context), IInterviewRepository
    {
        public async Task<List<Interview>> GetInterviewsWithJobDetailsAsync(int userId)
        {
            DateTime now = DateTime.UtcNow;
            DateTime nextWeek = now.AddDays(7);

            return await _context.Interviews
                .Include(i => i.JobApplication) 
                .Where(i => i.JobApplication.UserId == userId
                            && i.InterviewDate >= now
                            && i.InterviewDate <= nextWeek
                            && i.Status==InterviewStatus.Scheduled)
                .OrderBy(i => i.InterviewDate)
                .ToListAsync(); 
        }

        public async Task<List<Interview>> GetAllInterviewsByUserIdAsync(int userId, InterviewStatus? filterStatus)
        {
            var query = context.Interviews
                .Include(i => i.JobApplication)
                .Where(i => i.JobApplication.UserId == userId);

            if (filterStatus.HasValue)
            {
                query = query.Where(i => i.Status == filterStatus.Value);
            }

            var now = DateTime.UtcNow;

            query = query
                .OrderBy(i => i.InterviewDate < now)
                .ThenBy(i => i.InterviewDate);

            return await query.ToListAsync();
        }

        public async Task<Interview?> GetInterviewByIdWithJobAsync(int id)
        {
            return await context.Interviews
                .Include(i => i.JobApplication) 
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<bool> HasActiveInterviewForRoundAsync(int applicationId, string roundName)
        {

            string normalizedName = roundName.Trim().ToLower();

            return await context.Interviews
                .AnyAsync(i => i.ApplicationId == applicationId
                            && i.RoundName.ToLower() == normalizedName
                            && i.Status != InterviewStatus.Cancelled);
        }
    }
}
