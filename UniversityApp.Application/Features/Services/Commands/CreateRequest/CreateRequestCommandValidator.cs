using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace UniversityApp.Application.Features.Services.Commands.CreateRequest
{
    public class CreateRequestCommandValidator : AbstractValidator<CreateRequestCommand>
    {
        public CreateRequestCommandValidator()
        {
            RuleFor(x => x.ServiceId)
                .GreaterThan(0).WithMessage("ServiceId must be greater than 0.");
            RuleFor(x => x.StudentId)
                .GreaterThan(0).WithMessage("StudentId must be greater than 0.");
        }
    }
}
