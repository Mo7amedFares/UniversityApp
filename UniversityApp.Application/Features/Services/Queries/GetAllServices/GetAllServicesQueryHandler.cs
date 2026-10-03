using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using UniversityApp.Application.Common.Interfaces;
using UniversityApp.Application.Common.Models;

namespace UniversityApp.Application.Features.Services.Queries.GetAllServices
{
    public class GetAllServicesQueryHandler : IRequestHandler<GetAllServicesQuery, PagedResult<ServiceDto>>
    {
        readonly IApplicationDbContext _context;
        public GetAllServicesQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<PagedResult<ServiceDto>> Handle(GetAllServicesQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Services.AsNoTracking();

            // تطبيق البحث إذا تم تمريره
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var searchTerm = request.Search.Trim();
                query = query.Where(s => s.Title.Contains(searchTerm) || s.Description.Contains(searchTerm));
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderByDescending(s => s.CreatedAt)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(s => new ServiceDto
                {
                    Id = s.Id,
                    Title = s.Title,
                    Fees = s.Fees,
                    WorkDays = s.WorkDays
                })
                .ToListAsync(cancellationToken);

            return new PagedResult<ServiceDto>
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
