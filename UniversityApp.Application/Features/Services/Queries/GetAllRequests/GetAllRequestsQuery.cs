using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using UniversityApp.Application.Features.Requests.Queries.GetAllRequests;

namespace UniversityApp.Application.Features.Services.Queries.GetAllRequests
{
    public class GetAllRequestsQuery:IRequest<List<AdminRequestDto>>
    {

    }
}
