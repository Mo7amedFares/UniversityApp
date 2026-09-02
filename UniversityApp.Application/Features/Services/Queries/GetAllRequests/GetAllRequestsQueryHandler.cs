using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using UniversityApp.Application.Common.Interfaces;
using UniversityApp.Application.Features.Requests.Queries.GetAllRequests;

namespace UniversityApp.Application.Features.Services.Queries.GetAllRequests
{
    public class GetAllRequestsQueryHandler : IRequestHandler<GetAllRequestsQuery, List<AdminRequestDto>>
    {

        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUser;
        public GetAllRequestsQueryHandler(IApplicationDbContext context , ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }
        public async Task<List<AdminRequestDto>> Handle(GetAllRequestsQuery request, CancellationToken cancellationToken)
        {
            var requests = await _context.Requests.Select(r => new AdminRequestDto
            {
                RequestId = r.Id,
                // استخدمنا Navigation Properties للوصول للبيانات مباشرة
                StudentName = r.Student.Name, // تأكد أن اسم الخاصية لديك هو User أو Student
                NationalId = r.Student.NationalId,
                ServiceTitle = r.Service.Title,
                Status = r.Status,
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt

            }).OrderByDescending(r => r.UpdatedAt)
            .ToListAsync(cancellationToken);

            return requests;
        }
    }
}
