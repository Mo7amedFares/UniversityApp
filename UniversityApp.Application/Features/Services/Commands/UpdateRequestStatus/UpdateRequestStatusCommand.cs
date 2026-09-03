using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Text;
using UniversityApp.Domain.Enums;

namespace UniversityApp.Application.Features.Services.Commands.UpdateRequestStatus
{
    public class UpdateRequestStatusCommand:IRequest<bool>
    {
        [BindNever] // يمنع الـ Model Binder من البحث عنها في الـ Form
        public int RequestId { get; set; }
        public RequestStatus NewStatus { get; set; } 
    }
}
