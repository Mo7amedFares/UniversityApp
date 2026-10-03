using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UniversityApp.Application.Common.Interfaces;
using UniversityApp.Application.Common.Models;

namespace UniversityApp.Application.Features.Students.Queries.GetAllStudents
{
    public class GetAllStudentsQueryHandler : IRequestHandler<GetAllStudentsQuery, PagedResult<StudentDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetAllStudentsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<StudentDto>> Handle(GetAllStudentsQuery request, CancellationToken cancellationToken)
        {
            // نجلب الطلاب فقط (Role = 0)
            var query = _context.Users
                .AsNoTracking()
                .Where(u => (int)u.Role == 0);

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var search = request.Search.Trim();
                query = query.Where(u =>
                    u.Name.Contains(search) ||
                    u.NationalId.Contains(search) ||
                    (u.StudentCode != null && u.StudentCode.Contains(search)) ||
                    u.PhoneNumber.Contains(search));
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderByDescending(u => u.CreatedAt)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(u => new StudentDto
                {
                    Id = u.Id,
                    NationalId = u.NationalId,
                    PhoneNumber = u.PhoneNumber,
                    StudentCode = u.StudentCode,
                    Name = u.Name,
                    Role = (int)u.Role,
                    CreatedAt = u.CreatedAt
                })
                .ToListAsync(cancellationToken);

            return new PagedResult<StudentDto>
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