using MediatR;

namespace UniversityApp.Application.Features.Payments.Commands.CreateSession
{
    public class CreatePaymentSessionCommand : IRequest<string>
    {
        public int RequestId { get; set; }
    }
}