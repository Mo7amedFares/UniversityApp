using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using UniversityApp.Application.Features.Requests.Queries.GetMyRequests;

namespace UniversityApp.Application.Features.Services.Queries.GetMyRequests
{
    public class GetMyRequestsQuery : IRequest<List<RequestDto>>
    {
    }
}
