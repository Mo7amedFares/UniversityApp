using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using UniversityApp.Application.Common.Events;
using UniversityApp.Application.Common.Interfaces;
using UniversityApp.Domain.Entities;

namespace UniversityApp.Application.Features.Notifications.EventHandlers
{
    public class RequestStatusChangedEventHandler : INotificationHandler<RequestStatusChangedEvent>
    {
        private readonly IApplicationDbContext _context;
        public RequestStatusChangedEventHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Handle(RequestStatusChangedEvent notificationEvent, CancellationToken cancellationToken)
        {
            var notification = new Notification
            {
                UserId = notificationEvent.StudentId,
                RequestId = notificationEvent.RequestId,
                Title = "تحديث حالة الطلب",
                Message = $"تم تحديث حالة طلبك رقم {notificationEvent.RequestId} إلى: {notificationEvent.NewStatus}",
                IsRead = false
            };
             _context.Notifications.Add(notification);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
