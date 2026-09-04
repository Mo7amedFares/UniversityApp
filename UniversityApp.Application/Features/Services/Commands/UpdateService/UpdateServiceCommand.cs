using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace UniversityApp.Application.Features.Services.Commands.UpdateService
{
    public class UpdateServiceCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int WorkDays { get; set; }
        public decimal Fees { get; set; }
        public bool IsActive { get; set; }
        public bool HasRequiredFiles { get; set; }
    }
}
