using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobTracker.Domain.Enums;

namespace JobTracker.Application.DTO
{
    public class JobStatDto
    {
        public ApplicationStatus Status { get; set; }
        public int Count { get; set; }
    }
}
