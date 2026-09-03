using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using UniversityApp.Application.Common.Interfaces;
using UniversityApp.Domain.Entities;

namespace UniversityApp.Application.Features.Services.Commands.AddRequestNote
{
    public class AddRequestNoteCommandHandler : IRequestHandler<AddRequestNoteCommand, int>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _curentUser;
        private readonly IFileService _fileService;

        public AddRequestNoteCommandHandler(IApplicationDbContext context , ICurrentUserService curentUser, IFileService fileService)
        {
            _context = context;
            _curentUser = curentUser;
            _fileService = fileService;
        }

        public async Task<int> Handle(AddRequestNoteCommand request, CancellationToken cancellationToken)
        {
            if (_curentUser.UserId == null)
                throw new UnauthorizedAccessException("غير مصرح لك بالقيام بهذه العملية.");

            // يجب أن يحتوي الطلب على نص أو ملف على الأقل
            if (string.IsNullOrWhiteSpace(request.Content) && (request.File == null || request.File.Length == 0))
            {
                throw new ArgumentException("يجب كتابة ملاحظة أو إرفاق ملف.");
            }
            var requestEntity = await _context.Requests.FirstOrDefaultAsync(r => r.Id == request.RequestId, cancellationToken);
            if (requestEntity == null)
                throw new InvalidOperationException("Request not found.");
            
            bool isAdmin = _curentUser.Role.Contains("admin");
            if(!isAdmin && requestEntity.StudentId != _curentUser.UserId)
                throw new UnauthorizedAccessException("You are not authorized to add a note to this request.");



            string? fileUrl = null;
            if (request.File != null && request.File.Length > 0)
            {
                var allowedExtensions = new[] { ".pdf", ".jpg", ".jpeg", ".png" };

                // استخدام request.File.FileName بعد التأكد أن File ليس null
                var ext = Path.GetExtension(request.File.FileName).ToLowerInvariant();

                if (Array.IndexOf(allowedExtensions, ext) == -1)
                {
                    throw new ArgumentException("نوع الملف غير مسموح. الأنواع المسموحة هي: PDF, JPG, PNG");
                }

                using var stream = request.File.OpenReadStream();
                fileUrl = await _fileService.SaveFileAsync(stream, request.File.FileName, "request_notes", cancellationToken);
            }
            var note = new RequestNote
            {
                RequestId = request.RequestId,
                Content = request.Content,
                FileUrl = fileUrl,
                UserId = (int)_curentUser.UserId,
                SenderRole = isAdmin ? Domain.Enums.UserRole.admin : Domain.Enums.UserRole.student,
                CreatedAt = DateTime.UtcNow,
            };

            _context.RequestNotes.Add(note);
            await _context.SaveChangesAsync(cancellationToken);

            return note.Id;
        }
    }
}
