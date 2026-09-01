using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using UniversityApp.Application.Common.Interfaces;
using UniversityApp.Domain.Entities;

namespace UniversityApp.Application.Features.Services.Commands.CreateService
{
    public class CreateServiceCommandHandler : IRequestHandler<CreateServiceCommand, int>
    {
        IApplicationDbContext _context;

        public CreateServiceCommandHandler(IApplicationDbContext Context)
        {
            _context = Context;
        }


        public async Task<int> Handle(CreateServiceCommand request, CancellationToken cancellationToken)
        {
            var service = new Service
            {
                Title = request.Title,
                Description = request.Description,
                Fees = request.Fees,
                WorkDays = request.WorkDays,
                HasRequiredFiles = request.HasRequiredFiles
            };

            _context.Services.Add(service);
            await _context.SaveChangesAsync(cancellationToken);
            return service.Id;
        }
    }
}
