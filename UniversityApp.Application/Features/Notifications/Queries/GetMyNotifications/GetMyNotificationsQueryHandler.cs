using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using UniversityApp.Application.Common.Interfaces;
using UniversityApp.Application.Common.Models;

namespace UniversityApp.Application.Features.Notifications.Queries.GetMyNotifications
{
    public class GetMyNotificationsQueryHandler:IRequestHandler<GetMyNotificationsQuery, PagedResult<NotificationDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        public GetMyNotificationsQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<PagedResult<NotificationDto>> Handle(GetMyNotificationsQuery request, CancellationToken cancellationToken)
        {
            if (_currentUserService.UserId == null)
            {
                throw new Exception("User is not authenticated.");
            }

            var query = _context.Notifications
                .AsNoTracking()
                .Where(n => n.UserId == _currentUserService.UserId.Value);

            var totalCount = await query.CountAsync(cancellationToken);

            // جلب الإشعارات الأحدث أولاً مع تطبيق الـ Paging
            var items = await query
                .OrderByDescending(n => n.CreatedAt)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
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

            return new PagedResult<NotificationDto>
            {
                Items = items,
                Page = request.Page,
                PageSize = request.PageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize)
            };
        }
    }
}
