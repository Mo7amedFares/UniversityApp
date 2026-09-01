using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace UniversityApp.Application.Features.Services.Commands.CreateStudent
{
    public class CreateStudentCommand:IRequest<int>
    {
        public string Name { get; set; } = string.Empty;
        public string NationalId { get; set; } = string.Empty;
        public string StudentCode { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

    }
}
