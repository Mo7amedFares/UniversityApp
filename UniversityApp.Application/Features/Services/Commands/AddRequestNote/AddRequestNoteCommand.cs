using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Text;
using UniversityApp.Application.Common.Interfaces;

namespace UniversityApp.Application.Features.Services.Commands.AddRequestNote
{
    public class AddRequestNoteCommand : IRequest<int>
    {
        [BindNever] // يمنع الـ Model Binder من البحث عنها في الـ Form
        public int RequestId { get; set; }
        public string? Content { get; set; }

        public IFormFile? File { get; set; } // الملف الاختياري المرفوع
    }
}