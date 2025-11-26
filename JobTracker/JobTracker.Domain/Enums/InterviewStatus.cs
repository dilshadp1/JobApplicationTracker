using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Domain.Enums
{
    public enum InterviewStatus
    {
        Scheduled = 0,
        Completed = 1, 
        Cancelled = 2, 
        NoShow = 3
    }
}
