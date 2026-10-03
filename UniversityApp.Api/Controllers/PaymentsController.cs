using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversityApp.Application.Features.Payments.Commands.CreateSession;

namespace UniversityApp.Api.Controllers
{
    [Controller]
    [Route("api/[controller]")]
    public class PaymentsController: ControllerBase
    {
        private readonly IMediator _mediator;
        public PaymentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create-session")]
        public async Task<ActionResult<string>> CreateSession([FromBody] CreatePaymentSessionCommand command)
        {
            var paymentUrl = await _mediator.Send(command);

            // نرجع الرابط للـ Frontend لكي يقوم بتوجيه الطالب (Redirect) لصفحة البنك
            return Ok(new { url = paymentUrl });
        }
    }
}
