using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Domain.Enums
{
    public enum ApplicationStatus
    {
        Applied = 0,
        Interviewing = 1,
        OfferReceived = 2,
        Hired = 3,
        Declined = 4,
        Rejected = 5
    }
}
