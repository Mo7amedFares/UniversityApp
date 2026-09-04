using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace UniversityApp.Application.Features.Notifications.Commands.MarkAsRead
{
    public class MarkNotificationAsReadCommand : IRequest<bool>
    {
        public int NotificationId { get; set; }
    }
}
