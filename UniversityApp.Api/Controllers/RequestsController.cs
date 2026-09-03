using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversityApp.Application.Features.RequestNotes.Queries.GetRequestNotes;
using UniversityApp.Application.Features.Requests.Queries.GetAllRequests;
using UniversityApp.Application.Features.Requests.Queries.GetMyRequests;
using UniversityApp.Application.Features.Services.Commands.AddRequestNote;
using UniversityApp.Application.Features.Services.Commands.CreateRequest;
using UniversityApp.Application.Features.Services.Commands.UpdateRequestStatus;
using UniversityApp.Application.Features.Services.Queries.GetAllRequests;
using UniversityApp.Application.Features.Services.Queries.GetMyRequests;
using UniversityApp.Application.Features.Services.Queries.GetRequestNotes;
using UniversityApp.Domain.Enums;

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
        [Authorize(Roles = "admin")] // السر كله هنا!
        public async Task<ActionResult<List<AdminRequestDto>>> GetAllRequests()
        {
            var query = new GetAllRequestsQuery();
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpPost("{id}/notes")]
        [Authorize]
        [Consumes("multipart/form-data")] 
        public async Task<ActionResult<int>> AddNote([FromRoute] int id, [FromForm] AddRequestNoteCommand command)
        {
            command.RequestId = id;
            var noteId = await _mediator.Send(command);
            return Ok(new { NoteId = noteId });
        }

        [HttpGet("{id}/notes")]
        [Authorize]
        public async Task<ActionResult<List<RequestNoteDto>>> GetNotes([FromRoute] int id)
        {
            GetRequestNotesQuery query = new GetRequestNotesQuery(id);
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpPatch("{id}/status")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<bool>> UpdateRequestStatus([FromRoute] int id, [FromBody] RequestStatus status)
        {
            var command = new UpdateRequestStatusCommand
            {
                RequestId = id,
                NewStatus = status
            };
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
