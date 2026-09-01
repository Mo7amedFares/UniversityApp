using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UniversityApp.Application.Common.Interfaces;

namespace UniversityApp.Application.Features.Services.Commands.CreateStudent
{
    public class CreateStudentCommentHandler : IRequestHandler<CreateStudentCommand, int>
    {
        private readonly IApplicationDbContext _context;
        private readonly IPasswordHasher _passwordHasher;

        public CreateStudentCommentHandler(IApplicationDbContext context, IPasswordHasher passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public async Task<int> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
        {
            bool isNationalIdExists = await _context.Users.AnyAsync(u => u.NationalId == request.NationalId , cancellationToken);
            if (isNationalIdExists)
            {
                throw new InvalidOperationException("National ID already exists.");
            }

            bool isStudentCodeExists = await _context.Users.AnyAsync(u => u.StudentCode == request.StudentCode, cancellationToken);
            if (isStudentCodeExists)
            {           
                throw new InvalidOperationException("Student Code already exists.");
            }
            var newStudent = new Domain.Entities.User
            {
                Name = request.Name,
                NationalId = request.NationalId,
                StudentCode = request.StudentCode,
                PasswordHash = _passwordHasher.Hash(request.Password)
            };
            
            _context.Users.Add(newStudent);
            await _context.SaveChangesAsync(cancellationToken);
            
            // Continue with the rest of the logic
            return newStudent.Id;
        }
    }
}
