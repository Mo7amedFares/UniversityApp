using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversityApp.Application.Features.Services.Commands.CreateService;
using UniversityApp.Application.Features.Services.Commands.UpdateService;
using UniversityApp.Application.Features.Services.Queries.GetAllServices;

namespace UniversityApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServicesController : ControllerBase
    {
        IMediator _mediator;

        public ServicesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var query = new GetAllServicesQuery();
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<int>> Create([FromBody] CreateServiceCommand command)
        {
            var serviceId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetAll), new { id = serviceId }, serviceId);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<bool>> Update(int id ,[FromBody] UpdateServiceCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

    }
}
