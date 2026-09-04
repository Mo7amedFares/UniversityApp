using System.Collections.Generic;
using MediatR;

namespace UniversityApp.Application.Features.Notifications.Queries.GetMyNotifications
{
    public class GetMyNotificationsQuery : IRequest<List<NotificationDto>>
    {
        // لا نحتاج لإرسال UserId هنا لأننا سنقرأه من الـ Token مباشرة لضمان الأمان
    }
}