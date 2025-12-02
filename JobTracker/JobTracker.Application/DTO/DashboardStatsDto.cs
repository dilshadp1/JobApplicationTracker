using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Application.DTO
{
    public class DashboardStatsDto
    {
        public int TotalApplications { get; set; }
        public int Interviewing { get; set; }
        public int Offers { get; set; }
        public int Rejected { get; set; }
        public int Hired { get; set; }
        public int Declined { get; set; }

        public List<UpcomingInterviewDto> UpcomingInterviews { get; set; } = new();
        public List<RecentActivityDto> RecentActivities { get; set; } = new();


    }
}
