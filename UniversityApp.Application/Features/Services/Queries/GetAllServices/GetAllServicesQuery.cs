using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace UniversityApp.Application.Features.Services.Queries.GetAllServices
{
    public class GetAllServicesQuery : IRequest<List<ServiceDto>>
    {
    }
}
