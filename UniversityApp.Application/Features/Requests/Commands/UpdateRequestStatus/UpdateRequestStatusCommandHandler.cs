using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using UniversityApp.Application.Common.Events;
using UniversityApp.Application.Common.Interfaces;
using UniversityApp.Domain.Enums;

namespace UniversityApp.Application.Features.Requests.Commands.UpdateRequestStatus
{
    public class UpdateRequestStatusCommandHandler : IRequestHandler<UpdateRequestStatusCommand, bool>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMediator _mediator;
        public UpdateRequestStatusCommandHandler(IApplicationDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<bool> Handle(UpdateRequestStatusCommand request, CancellationToken cancellationToken)
        {
            var RequstEntity = await _context.Requests.FirstOrDefaultAsync(r => r.Id == request.RequestId, cancellationToken);
            if (RequstEntity == null)
            {
                throw new Exception($"Request with ID {request.RequestId} does not exist.");
            }

            RequstEntity.Status = request.NewStatus;
            RequstEntity.UpdatedAt = DateTime.UtcNow;

            int result = await _context.SaveChangesAsync(cancellationToken);

            if(result >0) 
                await _mediator.Publish(new RequestStatusChangedEvent(RequstEntity.Id, RequstEntity.StudentId, request.NewStatus), cancellationToken);

            return result > 0;
        }
    }
}
