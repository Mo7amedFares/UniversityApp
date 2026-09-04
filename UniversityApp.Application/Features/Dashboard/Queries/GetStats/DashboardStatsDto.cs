using System.Collections.Generic;

namespace UniversityApp.Application.Features.Dashboard.Queries.GetStats
{
    public class DashboardStatsDto
    {
        public int TotalRequests { get; set; }
        public int PendingRequests { get; set; }
        public int ApprovedRequests { get; set; }
        public int RejectedRequests { get; set; }
        public int RequiresActionRequests { get; set; }
        public int TotalActiveServices { get; set; }

        // (اختياري) يمكننا إرجاع أكثر الخدمات طلباً
        public List<ServiceStatDto> TopServices { get; set; } = new();
    }

    public class ServiceStatDto
    {
        public string ServiceName { get; set; } = string.Empty;
        public int RequestCount { get; set; }
    }
}