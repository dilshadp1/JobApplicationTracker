using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobTracker.Application.DTO;
using MediatR;

namespace JobTracker.Application.Query.GetDashboardStatsQuery
{
    public class GetDashboardStatsQuery : IRequest<DashboardStatsDto>
    {
    }
}
