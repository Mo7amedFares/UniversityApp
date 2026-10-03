using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using UniversityApp.Application.Common.Models;

namespace UniversityApp.Application.Features.Requests.Queries.GetMyRequests
{
    public class GetMyRequestsQuery : IRequest<PagedResult<RequestDto>>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 50;

        // فلاتر البحث اللي الإدمن هيستخدمها
        public string? Search { get; set; }
        public int? Status { get; set; }
    }
}
