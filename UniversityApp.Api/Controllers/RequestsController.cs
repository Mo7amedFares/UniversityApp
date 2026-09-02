using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversityApp.Application.Features.Requests.Queries.GetAllRequests;
using UniversityApp.Application.Features.Requests.Queries.GetMyRequests;
using UniversityApp.Application.Features.Services.Commands.CreateRequest;
using UniversityApp.Application.Features.Services.Queries.GetAllRequests;
using UniversityApp.Application.Features.Services.Queries.GetMyRequests;

namespace UniversityApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RequestsController : ControllerBase
    {
        IMediator _mediator;
        public RequestsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [AllowAnonymous] // مسموح للعامة
        public async Task<ActionResult<int>> CreateRequest([FromBody] CreateRequestCommand command)
        {
            var requestId = await _mediator.Send(command);

            return Ok(new { RequestId = requestId });
        }

        [HttpGet("my-requests")]
        [Authorize] // هذا يحمي المسار ويجبر السيرفر على قراءة التوكن
        public async Task<ActionResult<List<RequestDto>>> GetMyRequests()
        {
            var query = new GetMyRequestsQuery();
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpGet("all")]
        [Authorize(Roles = "Admin")] // السر كله هنا!
        public async Task<ActionResult<List<AdminRequestDto>>> GetAllRequests()
        {
            var query = new GetAllRequestsQuery();
            var result = await _mediator.Send(query);

            return Ok(result);
        }
    }
}
