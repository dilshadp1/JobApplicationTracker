using JobTracker.Domain.Entities;
using JobTracker.Domain.Enums;

namespace JobTracker.Application.Interfaces
{
    public interface IJobApplicationRepository : IGenericRepository<JobApplication>
    {
        Task<List<JobApplication>> GetApplicationsWithDetailsAsync(int userId);
        Task<Dictionary<ApplicationStatus, int>> GetJobStatsByUserIdAsync(int userId);
        Task<List<JobApplication>> GetRecentApplicationsAsync(int userId, int count);
    }
}
