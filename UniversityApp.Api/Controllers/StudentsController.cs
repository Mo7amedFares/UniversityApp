using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;
using UniversityApp.Application.Features.Services.Commands.CreateStudent;

namespace UniversityApp.Api.Controllers
{
    [Controller]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        IMediator _mediator;
        public StudentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStudentCommand command)
        {
            var studentId = await _mediator.Send(command);
            return Ok(studentId);
        }
    }
}
