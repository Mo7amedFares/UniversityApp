using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;
using UniversityApp.Application.Common.Models;
using UniversityApp.Application.Features.Students.Commands.CreateStudent;
using UniversityApp.Application.Features.Students.Queries.GetAllStudents;

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
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create([FromBody] CreateStudentCommand command)
        {
            var studentId = await _mediator.Send(command);
            return Ok(studentId);
        }

        [HttpGet("all")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<PagedResult<StudentDto>>> GetAllStudents(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 50,
            [FromQuery] string? search = null)
        {
            var query = new GetAllStudentsQuery
            {
                Page = page,
                PageSize = pageSize,
                Search = search
            };
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
