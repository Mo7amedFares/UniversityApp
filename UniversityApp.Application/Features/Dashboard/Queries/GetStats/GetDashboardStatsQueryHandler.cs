using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using UniversityApp.Application.Common.Interfaces;
using UniversityApp.Domain.Enums;

namespace UniversityApp.Application.Features.Dashboard.Queries.GetStats
{
    public class GetDashboardStatsQueryHandler : IRequestHandler<GetDashboardStatsQuery, DashboardStatsDto>
    {

        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public GetDashboardStatsQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<DashboardStatsDto> Handle(GetDashboardStatsQuery request, CancellationToken cancellationToken)
        {
            if(_currentUserService.UserId == null)
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }

            var RequestSatas = await _context.Requests
                .GroupBy(r => r.Status)
                .Select(g => new
                {
                    Status = g.Key,
                    Count = g.Count()
                })
                .ToListAsync(cancellationToken);



            var dashboardStats = new DashboardStatsDto()
            {
                TotalRequests = RequestSatas.Sum(r => r.Count),

                PendingRequests = RequestSatas.FirstOrDefault(r => r.Status == RequestStatus.Pending)?.Count ?? 0,
                CompletedRequests = RequestSatas.FirstOrDefault(r => r.Status == RequestStatus.Completed)?.Count ?? 0,
                RejectedRequests = RequestSatas.FirstOrDefault(r => r.Status == RequestStatus.Rejected)?.Count ?? 0,
                ProcessingRequests = RequestSatas.FirstOrDefault(r => r.Status == RequestStatus.Processing)?.Count ?? 0,

                
            };

            dashboardStats.TotalActiveServices = await _context.Services.CountAsync(r => r.IsActive, cancellationToken);

            dashboardStats.TopServices = await _context.Requests
                .Where(r => r.Status == RequestStatus.Completed)
                .Include(r => r.Service)
                .GroupBy(r => r.Service!.Title)
                .Select(g => new ServiceStatDto
                {
                    ServiceName = g.Key,
                    RequestCount = g.Count()
                })
                .OrderByDescending(s => s.RequestCount)
                .Take(5)
                .ToListAsync(cancellationToken);



            return dashboardStats;


        }
    }
}
