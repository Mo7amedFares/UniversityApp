using System;

namespace UniversityApp.Application.Features.Notifications.Queries.GetMyNotifications
{
    public class NotificationDto
    {
        public int Id { get; set; }
        public int? RequestId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}