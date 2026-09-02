using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens; // لـ JwtRegisteredClaimNames
using UniversityApp.Application.Common.Interfaces;

namespace UniversityApp.Api.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int? UserId
        {
            get
            {
                // نستخرج الـ Id بناءً على ما كتبناه في JwtProvider
                var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(JwtRegisteredClaimNames.Sub);
                return userIdClaim != null ? int.Parse(userIdClaim.Value) : null;
            }
        }

        public string? NationalId => _httpContextAccessor.HttpContext?.User?.FindFirst("national_id")?.Value;

        public string? Role => _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Role)?.Value;

        public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
    }
}