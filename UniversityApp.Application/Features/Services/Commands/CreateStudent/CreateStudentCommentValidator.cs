using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace UniversityApp.Application.Features.Services.Commands.CreateStudent
{
    public class CreateStudentCommentValidator:AbstractValidator<CreateStudentCommand>
    {
        public CreateStudentCommentValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(150).WithMessage("Name cannot exceed 150 characters.");
            RuleFor(x => x.NationalId)
                .NotEmpty().WithMessage("National ID is required.")
                .Matches(@"^\d{14}$").WithMessage("National ID must be a 14-digit number.");
            RuleFor(x => x.StudentCode)
                .NotEmpty().WithMessage("Student Code is required.")
                .MaximumLength(20).WithMessage("Student Code cannot exceed 20 characters.");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
        }
    }
}
