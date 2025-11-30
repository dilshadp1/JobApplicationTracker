using JobTracker.Application.Interfaces;
using JobTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Infrastructure.Repositories
{
    public class JobApplicationRepository(GenericRepository<JobApplication>) : IJobApplicationRepository
    {
    }
}
