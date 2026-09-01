using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using UniversityApp.Application.Common.Interfaces;

namespace UniversityApp.Application.Features.Services.Queries.Login
{
    public class LoginQueryHandler : IRequestHandler<LoginQuery, string>
    {
        private readonly IApplicationDbContext _context;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtProvider _jwtProvider;
        public LoginQueryHandler(IApplicationDbContext context, IPasswordHasher passwordHasher, IJwtProvider jwtProvider)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _jwtProvider = jwtProvider;
        }
        public async Task<string> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.NationalId == request.NationalId);
            if (user == null)
            {
                throw new UnauthorizedAccessException("National ID or password is incorrect.");
            }
            if (!_passwordHasher.Verify(request.Password , user.PasswordHash))
            {
                throw new UnauthorizedAccessException("National ID or password is incorrect.");
            }
            return _jwtProvider.Generate(user);
        }
    }
}
