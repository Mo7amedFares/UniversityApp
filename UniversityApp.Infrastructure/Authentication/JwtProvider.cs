using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UniversityApp.Application.Common.Interfaces;
using UniversityApp.Domain.Entities;

namespace UniversityApp.Infrastructure.Authentication
{
    public class JwtProvider : IJwtProvider
    {
        private readonly JwtSettings _jwtSettings;

        public JwtProvider(IOptions<JwtSettings> jwtOptions)
        {
            _jwtSettings = jwtOptions.Value;
        }

        public string Generate(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim("national_id", user.NationalId),
                new Claim(ClaimTypes.Role,user.Role.ToString()) // لتطبيق الصلاحيات لاحقاً
            };

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey)),
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _jwtSettings.Issuer,
                _jwtSettings.Audience,
                claims,
                null,
                DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
                signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}