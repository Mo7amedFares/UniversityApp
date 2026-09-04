using MediatR;
using Microsoft.AspNetCore.Mvc;
using UniversityApp.Application.Features.Login.Queries;

namespace UniversityApp.Api.Controllers
{
    [Controller]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IMediator _mediator;
        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login([FromBody] LoginQuery query)
        {
            var token = await _mediator.Send(query);
            return Ok(new { Token = token });
        }

    }
}
