using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace UniversityApp.Application.Features.Requests.Queries.GetAllRequests
{
    public class GetAllRequestsQuery:IRequest<List<AdminRequestDto>>
    {

    }
}
