using FluentValidation;
using FluentValidation.Validators;
using System;
using System.Collections.Generic;
using System.Text;

namespace UniversityApp.Application.Features.Services.Commands.UpdateRequestStatus
{
    public class UpdateRequestStatusCommandValidator : AbstractValidator<UpdateRequestStatusCommand>
    {
        public UpdateRequestStatusCommandValidator() { 
            RuleFor(x => x.RequestId)
                .GreaterThan(0)
                .WithMessage("RequestId must be greater than 0.");

            RuleFor(x => x.NewStatus)
                .IsInEnum()
                .WithMessage("NewStatus must be a valid enum value like 'Pending'= 1 , 'In Progress' = 2, 'Completed' = 3, or 'Rejected' = 4.");
        }
    }
}
