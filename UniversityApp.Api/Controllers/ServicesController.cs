using MediatR;
using Microsoft.AspNetCore.Mvc;
using UniversityApp.Application.Features.Services.Commands.CreateService;
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
        public async Task<IActionResult> GetAllServices(CancellationToken cancellationToken)
        {
            var query = new GetAllServicesQuery();
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] CreateServiceCommand command)
        {
           var serviceId = await _mediator.Send(command);
            return Ok(serviceId);
        }
    }
}
