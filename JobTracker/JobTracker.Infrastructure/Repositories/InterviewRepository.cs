using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobTracker.Application.Interfaces;
using JobTracker.Domain.Entities;
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
                            && i.InterviewDate <= nextWeek)
                .OrderBy(i => i.InterviewDate)
                .ToListAsync(); 
        }

        public async Task<List<Interview>> GetAllInterviewsByUserIdAsync(int userId)
        {
            return await context.Interviews
                .Include(i => i.JobApplication) // Include this so we can show Company Name
                .Where(i => i.JobApplication.UserId == userId)
                .OrderByDescending(i => i.InterviewDate) // Show newest first
                .ToListAsync();
        }

        public async Task<Interview?> GetInterviewByIdWithJobAsync(int id)
        {
            return await context.Interviews
                .Include(i => i.JobApplication) // This performs the JOIN
                .FirstOrDefaultAsync(i => i.Id == id);
        }
    }
}
