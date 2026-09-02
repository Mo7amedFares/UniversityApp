using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace UniversityApp.Application.Features.Services.Commands.CreateRequest
{
    public class CreateRequestCommand : IRequest<int>
    {
        public int ServiceId { get; set; }
        public string NationalId { get; set; } = string.Empty;
    }
}
