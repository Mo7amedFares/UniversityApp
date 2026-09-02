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

            // 2. البحث عن الطالب بالرقم القومي (لأن المستخدم Guest)
            var student = await _context.Users
                .AsNoTracking()
                .SingleOrDefaultAsync(s => s.NationalId == request.NationalId, cancellationToken);

            if (student == null)
            {
                throw new Exception($"National ID not found: {request.NationalId}");
            }

            bool isAlredyRequestPending = await _context.Requests
                .AnyAsync(r => r.ServiceId == request.ServiceId
                && r.StudentId == student.Id 
                && r.Status == Domain.Enums.RequestStatus.Pending
                , cancellationToken);

            if(isAlredyRequestPending)
            {
                throw new Exception($"Request for Service with ID {request.ServiceId} and National ID {request.NationalId} is already pending.");
            }

            var requestEntity = new Request
            {
                ServiceId = request.ServiceId,
                StudentId = student.Id,
                Status = Domain.Enums.RequestStatus.Pending
            };

            _context.Requests.Add(requestEntity);
             await _context.SaveChangesAsync(cancellationToken);
            return requestEntity.Id;
        }
    }
}
