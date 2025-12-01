using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobTracker.Domain.Entities;

namespace JobTracker.Application.Interfaces
{
    public interface IInterviewRepository : IGenericRepository<Interview>
    {
        Task<List<Interview>> GetInterviewsWithJobDetailsAsync(int userId);
    }
}
