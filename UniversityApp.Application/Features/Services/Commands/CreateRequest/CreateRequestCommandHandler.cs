using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using UniversityApp.Application.Common.Interfaces;
using UniversityApp.Domain.Entities;

namespace UniversityApp.Application.Features.Services.Commands.CreateRequest
{
    public class CreateRequestCommandHandler : IRequestHandler<CreateRequestCommand, int>
    {
        IApplicationDbContext _context;
        public CreateRequestCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateRequestCommand request, CancellationToken cancellationToken)
        {
            bool serviceExists = await _context.Services.AnyAsync(s => s.Id == request.ServiceId, cancellationToken);

            if(!serviceExists)
            {
                throw new Exception($"Service with ID {request.ServiceId} does not exist.");
            }

            bool studentExists = await _context.Users.AnyAsync(s => s.Id == request.StudentId, cancellationToken);

            if(!studentExists)
            {
                throw new Exception($"Student with ID {request.StudentId} does not exist.");
            }

            bool isAlredyRequestPending = await _context.Requests
                .AnyAsync(r => r.ServiceId == request.ServiceId
                && r.StudentId == request.StudentId 
                && r.Status == Domain.Enums.RequestStatus.Pending
                , cancellationToken);

            if(isAlredyRequestPending)
            {
                throw new Exception($"Request for Service with ID {request.ServiceId} and Student with ID {request.StudentId} is already pending.");
            }

            var requestEntity = new Request
            {
                ServiceId = request.ServiceId,
                StudentId = request.StudentId,
                Status = Domain.Enums.RequestStatus.Pending
            };

            _context.Requests.Add(requestEntity);
            int RequstId = await _context.SaveChangesAsync(cancellationToken);
            return RequstId;
        }
    }
}
