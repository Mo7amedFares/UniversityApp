using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversityApp.Application.Features.Services.Commands.CreateRequest;

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
        public async Task<IActionResult> CreateRequest([FromBody] CreateRequestCommand command)
        {
            int requestId = await _mediator.Send(command);
            return Ok(requestId);
        }
    }
}
