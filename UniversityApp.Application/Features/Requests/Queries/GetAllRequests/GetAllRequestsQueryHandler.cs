using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using UniversityApp.Application.Common.Interfaces;
using UniversityApp.Application.Common.Models;
using UniversityApp.Domain.Enums;

namespace UniversityApp.Application.Features.Requests.Queries.GetAllRequests
{
    public class GetAllRequestsQueryHandler : IRequestHandler<GetAllRequestsQuery, PagedResult<AdminRequestDto>>
    {

        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUser;
        public GetAllRequestsQueryHandler(IApplicationDbContext context , ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task<PagedResult<AdminRequestDto>> Handle(GetAllRequestsQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Requests
                .Include(r => r.Student)
                .Include(r => r.Service)
                .AsNoTracking()
                .AsQueryable();

            if (request.Status.HasValue)
            {
                query = query.Where(r => (int)r.Status == request.Status.Value);
            }

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var searchTrim = request.Search.Trim();
                query = query.Where(r => 
                    r.Student.Name.Contains(searchTrim) ||
                    r.Student.NationalId.Contains(searchTrim) ||
                    (r.Student.StudentCode != null && r.Student.StudentCode.Contains(searchTrim)));
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(r => r.CreatedAt)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(r => new AdminRequestDto
                {
                    RequestId = r.Id,
                    StudentName = r.Student.Name,
                    NationalId = r.Student.NationalId,
                    ServiceTitle = r.Service.Title,
                    Status = (RequestStatus)r.Status,
                    CreatedAt = r.CreatedAt,
                    UpdatedAt = r.UpdatedAt
                })
                .ToListAsync(cancellationToken);

            return new PagedResult<AdminRequestDto>
            {
                Items = items,
                Page = request.Page,
                PageSize = request.PageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize)
            };
        }
    }
}
