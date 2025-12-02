using JobTracker.Domain.Entities;
using JobTracker.Domain.Enums;

namespace JobTracker.Application.Interfaces
{
    public interface IJobApplicationRepository : IGenericRepository<JobApplication>
    {
        Task<List<JobApplication>> GetJobApplicationsWithDetailsAsync(int userId);
        Task<JobApplication?> GetJobApplicationByIdWithDetailsAsync(int jobId, int userId);
        Task<Dictionary<ApplicationStatus, int>> GetJobStatsByUserIdAsync(int userId);
        Task<bool> IsJobOwnedByUserAsync(int jobId, int userId);
        Task<JobApplication?> GetJobWithOfferAsync(int jobId);
        Task<List<JobApplication>> GetRecentApplicationsAsync(int userId, int count);
    }
}
