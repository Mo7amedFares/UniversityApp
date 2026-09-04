using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using UniversityApp.Application.Common.Interfaces;

namespace UniversityApp.Application.Features.Services.Commands.UpdateService
{
    public class UpdateServiceCommandHandler:IRequestHandler<UpdateServiceCommand , bool>
    {
        private readonly IApplicationDbContext _context;

        public UpdateServiceCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(UpdateServiceCommand request, CancellationToken cancellationToken)
        {
            var service = await _context.Services.FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);
            if (service == null)
            {
                throw new KeyNotFoundException($"Service with Id {request.Id} not found.");
            }

            service.Title = request.Title;
            service.Description = request.Description;
            service.IsActive = request.IsActive;
            service.WorkDays = request.WorkDays;
            service.Fees = request.Fees;
            service.HasRequiredFiles = request.HasRequiredFiles;

            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
