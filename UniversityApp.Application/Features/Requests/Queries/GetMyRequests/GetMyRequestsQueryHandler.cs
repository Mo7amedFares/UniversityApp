using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using UniversityApp.Application.Common.Interfaces;
using UniversityApp.Application.Common.Models;

namespace UniversityApp.Application.Features.Requests.Queries.GetMyRequests
{
    public class GetMyRequestsQueryHandler : IRequestHandler<GetMyRequestsQuery, PagedResult<RequestDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public GetMyRequestsQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<PagedResult<RequestDto>> Handle(GetMyRequestsQuery request, CancellationToken cancellationToken)
        {
            if(_currentUserService.UserId == null)
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }

            var query = _context.Requests
               .Where(r => r.StudentId == _currentUserService.UserId)
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
                .Select(r => new RequestDto
                {
                    Id = r.Id,
                    ServiceTitle = r.Service.Title,
                    Status = r.Status,
                    CreatedAt = r.CreatedAt,
                    UpdatedAt = r.UpdatedAt
                }).OrderByDescending(r => r.UpdatedAt)
                .ToListAsync(cancellationToken);

            return new PagedResult<RequestDto>
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
