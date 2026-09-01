using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace UniversityApp.Application.Features.Services.Commands.CreateService
{
    public class CreateServiceCommandValidator : AbstractValidator<CreateServiceCommand>
    {
        public CreateServiceCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(200).WithMessage("Title cannot exceed 200 characters.");
            RuleFor(x => x.Fees)
                .GreaterThanOrEqualTo(0).WithMessage("Fees must be a non-negative value.");
            RuleFor(x => x.WorkDays)
                .GreaterThan(0).WithMessage("WorkDays must be greater than zero.");
        }
    }
}
