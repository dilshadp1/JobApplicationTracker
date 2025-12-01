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
            var now = DateTime.UtcNow;
            var nextWeek = now.AddDays(7);

            return await context.Interviews
                .Include(i => i.JobApplication) 
                .Where(i => i.JobApplication.UserId == userId
                            && i.InterviewDate >= now
                            && i.InterviewDate <= nextWeek)
                .OrderBy(i => i.InterviewDate)
                .ToListAsync(); 
        }
    }
}
