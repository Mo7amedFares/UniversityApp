using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UniversityApp.Application.Common.Events;
using UniversityApp.Application.Common.Interfaces;
using UniversityApp.Domain.Entities;

namespace UniversityApp.Application.Features.Notifications.EventHandlers
{
    public class RequestNoteAddedEventHandler : INotificationHandler<RequestNoteAddedEvent>
    {
        private readonly IApplicationDbContext _context;

        public RequestNoteAddedEventHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Handle(RequestNoteAddedEvent notificationEvent, CancellationToken cancellationToken)
        {
            var notification = new Notification
            {
                UserId = notificationEvent.StudentId,
                RequestId = notificationEvent.RequestId,
                Title = "ملاحظة جديدة",
                Message = $"قام موظف الشؤون بإضافة ملاحظة جديدة على طلبك رقم {notificationEvent.RequestId}.",
                IsRead = false
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}