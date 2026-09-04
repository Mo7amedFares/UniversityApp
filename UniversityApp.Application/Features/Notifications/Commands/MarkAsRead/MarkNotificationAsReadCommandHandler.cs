using MediatR;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using UniversityApp.Application.Common.Interfaces;

namespace UniversityApp.Application.Features.Notifications.Commands.MarkAsRead
{
    public class MarkNotificationAsReadCommandHandler:IRequestHandler<MarkNotificationAsReadCommand, bool>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        public MarkNotificationAsReadCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }
        public async Task<bool> Handle(MarkNotificationAsReadCommand request, CancellationToken cancellationToken)
        {
            var notification = await _context.Notifications.FindAsync(request.NotificationId);
            if (notification == null)
            {
                throw new KeyNotFoundException($"Notification with ID {request.NotificationId} not found.");
            }
            if(_currentUserService.UserId != notification.UserId)
            {
                throw new UnauthorizedAccessException("You are not authorized to mark this notification as read.");
            }
            notification.IsRead = true;
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
