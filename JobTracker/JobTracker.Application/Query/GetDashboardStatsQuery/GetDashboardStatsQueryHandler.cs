using JobTracker.Application.DTO;
using JobTracker.Application.Interfaces;
using JobTracker.Domain.Entities;
using JobTracker.Domain.Enums;
using MediatR;

namespace JobTracker.Application.Query.GetDashboardStatsQuery
{
    public class GetDashboardStatsQueryHandler : IRequestHandler<GetDashboardStatsQuery, DashboardStatsDto>
    {
        private readonly IJobApplicationRepository _jobRepo;
        private readonly IInterviewRepository _interviewRepo;
        private readonly ICurrentUserService _currentUser;

        public GetDashboardStatsQueryHandler(IJobApplicationRepository jobRepo, IInterviewRepository interviewRepo, ICurrentUserService currentUser)
        {
            _jobRepo = jobRepo;
            _interviewRepo = interviewRepo;
            _currentUser = currentUser;
        }
        public async Task<DashboardStatsDto> Handle(GetDashboardStatsQuery request, CancellationToken cancellationToken)
        {
            int userId = _currentUser.UserId;

            Dictionary<ApplicationStatus, int> statsMap = await _jobRepo.GetJobStatsByUserIdAsync(userId);

            int GetCount(ApplicationStatus status) => statsMap.GetValueOrDefault(status, 0);
            int totalApplications = statsMap.Values.Sum();

            List<Interview> interviews = await _interviewRepo.GetInterviewsWithJobDetailsAsync(userId);

            List<UpcomingInterviewDto> upcomingInterviews = interviews.Select(i => new UpcomingInterviewDto
            {
                InterviewId = i.Id,
                Date = i.InterviewDate,
                RoundName = i.RoundName,
                Mode = i.Mode.ToString(),
                Company = i.JobApplication.Company,
                Position = i.JobApplication.Position
            }).ToList();

            return new DashboardStatsDto
            {
                TotalApplications = totalApplications,
                Interviewing = GetCount(ApplicationStatus.Interviewing),
                Offers = GetCount(ApplicationStatus.OfferReceived),
                Rejected = GetCount(ApplicationStatus.Rejected),
                Hired = GetCount(ApplicationStatus.Hired),
                Declined = GetCount(ApplicationStatus.Declined),
                UpcomingInterviews = upcomingInterviews
            };
        }
    }
}
