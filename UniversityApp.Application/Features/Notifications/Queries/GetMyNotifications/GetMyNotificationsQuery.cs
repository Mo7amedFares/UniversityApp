using System.Collections.Generic;
using MediatR;
using UniversityApp.Application.Common.Models;

namespace UniversityApp.Application.Features.Notifications.Queries.GetMyNotifications
{
    public class GetMyNotificationsQuery : IRequest<PagedResult<NotificationDto>>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 50;
    }
}