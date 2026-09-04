using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversityApp.Application.Features.Notifications.Commands.MarkAsRead;
using UniversityApp.Application.Features.Notifications.Queries.GetMyNotifications;

namespace UniversityApp.Api.Controllers
{
    [Controller]
    [Route("api/[controller]")]
    [Authorize]
    public class NotificationsController : ControllerBase
    {
        IMediator _mediator;
        public NotificationsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet("my-notifications")]
        public async Task<ActionResult<IEnumerable<NotificationDto>>> GetMyNotifications()
        {
            var notifications = await _mediator.Send(new GetMyNotificationsQuery());
            return Ok(notifications);
        }
        [HttpPatch("mark-as-read/{notificationId}")]
        public async Task<IActionResult> MarkAsRead(int notificationId)
        {
            var result = await _mediator.Send(new MarkNotificationAsReadCommand { NotificationId = notificationId });
            if (result)
                return Ok();
            else
                return NotFound();
        }
    }
}
