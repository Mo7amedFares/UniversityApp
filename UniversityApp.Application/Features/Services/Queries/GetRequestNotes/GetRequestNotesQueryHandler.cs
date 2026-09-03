using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using UniversityApp.Application.Common.Interfaces;
using UniversityApp.Application.Features.RequestNotes.Queries.GetRequestNotes;

namespace UniversityApp.Application.Features.Services.Queries.GetRequestNotes
{
    public class GetRequestNotesQueryHandler : IRequestHandler<GetRequestNotesQuery, List<RequestNoteDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUser;

        public GetRequestNotesQueryHandler(IApplicationDbContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }
        public async Task<List<RequestNoteDto>> Handle(GetRequestNotesQuery request, CancellationToken cancellationToken)
        {
            if(!_currentUser.UserId.HasValue)
            {
                throw new InvalidOperationException("User is not authenticated.");
            }
            var TargetRequest = _context.Requests
                .AsNoTracking()
                .Select(r => new {r.Id , r.StudentId})
                .FirstOrDefaultAsync(r => r.Id == request.RequestId, cancellationToken);

            if(TargetRequest==null)
                throw new InvalidOperationException("Request not found.");

            bool isAdmin = _currentUser.Role?.Contains("admin") ?? false;

            if(!isAdmin && TargetRequest.Result.StudentId != _currentUser.UserId)
                throw new UnauthorizedAccessException("You are not authorized to view notes for this request.");

            var RequestNotes = await _context.RequestNotes
                .AsNoTracking()
                .Where(n => n.RequestId == request.RequestId)
                .Select(n => new RequestNoteDto
                {
                    Id = n.Id,
                    Content = n.Content,
                    FileUrl = n.FileUrl,
                    SenderRole = n.SenderRole.ToString(),
                    CreatedAt = n.CreatedAt
                })
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync(cancellationToken);

            return RequestNotes;
        }
    }
}
