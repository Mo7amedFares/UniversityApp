using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using UniversityApp.Application.Common.Interfaces;

namespace UniversityApp.Application.Features.Notifications.Queries.GetMyNotifications
{
    public class GetMyNotificationsQueryHandler:IRequestHandler<GetMyNotificationsQuery, List<NotificationDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        public GetMyNotificationsQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<List<NotificationDto>> Handle(GetMyNotificationsQuery request, CancellationToken cancellationToken)
        {
            if (_currentUserService.UserId == null)
            {
                throw new Exception("User is not authenticated.");
            }

            var notifications = await _context.Notifications
                .AsNoTracking()
                .Where(n => n.UserId == _currentUserService.UserId)
                .Select(n => new NotificationDto
                {
                    Id = n.Id,
                    RequestId = n.RequestId,
                    Title = n.Title,
                    Message = n.Message,
                    IsRead = n.IsRead,
                    CreatedAt = n.CreatedAt
                })
                .ToListAsync(cancellationToken);
            
            return notifications;
        }
    }
}
