using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobTracker.Domain.Entities;
using JobTracker.Domain.Enums;

namespace JobTracker.Application.Interfaces
{
    public interface IInterviewRepository : IGenericRepository<Interview>
    {
        Task<List<Interview>> GetInterviewsWithJobDetailsAsync(int userId);
        Task<List<Interview>> GetAllInterviewsByUserIdAsync(int userId, InterviewStatus? filterStatus);
        Task<Interview?> GetInterviewByIdWithJobAsync(int id);
        Task<bool> HasActiveInterviewForRoundAsync(int applicationId, string roundName);
    }
}
