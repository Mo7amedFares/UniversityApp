using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using UniversityApp.Application.Common.Interfaces;
using UniversityApp.Application.Features.Requests.Queries.GetMyRequests;

namespace UniversityApp.Application.Features.Services.Queries.GetMyRequests
{
    public class GetMyRequestsQueryHandler : IRequestHandler<GetMyRequestsQuery, List<RequestDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public GetMyRequestsQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<List<RequestDto>> Handle(GetMyRequestsQuery request, CancellationToken cancellationToken)
        {
            if(_currentUserService.UserId == null)
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }

            var myRequests = await _context.Requests
                .Where(r => r.StudentId == _currentUserService.UserId)
                .Select(r => new RequestDto
                {
                    Id = r.Id,
                    ServiceTitle = r.Service.Title,
                    Status = r.Status,
                    CreatedAt = r.CreatedAt,
                    UpdatedAt = r.UpdatedAt
                }).OrderByDescending(r => r.UpdatedAt)
                .ToListAsync(cancellationToken);

            return myRequests;
        }
    }
}
