using JobTracker.Domain.Entities;

namespace JobTracker.Application.Interfaces
{
    public interface IJobApplicationRepository : IGenericRepository<JobApplication>
    {
        Task<List<JobApplication>> GetApplicationsWithDetailsAsync(int userId);
    }
}
