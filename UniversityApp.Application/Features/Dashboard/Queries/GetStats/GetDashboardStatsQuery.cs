using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace UniversityApp.Application.Features.Dashboard.Queries.GetStats
{
   public class GetDashboardStatsQuery:IRequest<DashboardStatsDto>
    {
    }
}
