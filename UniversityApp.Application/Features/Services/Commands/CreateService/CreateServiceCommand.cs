using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace UniversityApp.Application.Features.Services.Commands.CreateService
{
    public class CreateServiceCommand:IRequest<int>
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Fees { get; set; }
        public int WorkDays { get; set; }
        public bool HasRequiredFiles { get; set; }
    }
}
