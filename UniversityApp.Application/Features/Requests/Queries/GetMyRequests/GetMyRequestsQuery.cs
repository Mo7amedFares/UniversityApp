using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace UniversityApp.Application.Features.Requests.Queries.GetMyRequests
{
    public class GetMyRequestsQuery : IRequest<List<RequestDto>>
    {
    }
}
