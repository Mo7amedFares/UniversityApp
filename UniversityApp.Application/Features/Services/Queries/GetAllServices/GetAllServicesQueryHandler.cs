using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using UniversityApp.Application.Common.Interfaces;

namespace UniversityApp.Application.Features.Services.Queries.GetAllServices
{
    public class GetAllServicesQueryHandler : IRequestHandler<GetAllServicesQuery, List<ServiceDto>>
    {
        readonly IApplicationDbContext _context;
        public GetAllServicesQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public Task<List<ServiceDto>> Handle(GetAllServicesQuery request, CancellationToken cancellationToken)
        {
            var services = _context.Services.Select(s => new ServiceDto
            {
                Id = s.Id,
                Title = s.Title,
                Fees = s.Fees,
                WorkDays = s.WorkDays
            }).ToListAsync(cancellationToken);

            return services;
        }
    }
}
