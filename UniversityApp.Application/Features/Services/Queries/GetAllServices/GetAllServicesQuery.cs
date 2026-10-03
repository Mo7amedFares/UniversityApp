using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using UniversityApp.Application.Common.Models;

namespace UniversityApp.Application.Features.Services.Queries.GetAllServices
{
    public class GetAllServicesQuery : IRequest<PagedResult<ServiceDto>>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 50;
        public string? Search { get; set; }
    }
}
