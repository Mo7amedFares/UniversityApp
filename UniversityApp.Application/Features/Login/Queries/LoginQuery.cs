using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace UniversityApp.Application.Features.Login.Queries
{
    public class LoginQuery : IRequest<string>
    {
        public string NationalId { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
