using JobTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Application.Interfaces
{
    public interface IJobApplicationRepository : IGenericRepository<JobApplication>
    {
        Task<List<JobApplication>> GetApplicationsWithDetailsAsync(int userId);
    }
}
