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
                var user = _httpContextAccessor.HttpContext?.User;
                if (user == null) return null;

                // نبحث بكل الأشكال المحتملة للـ User Id:
                var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)
                               ?? user.FindFirst("sub")
                               ?? user.FindFirst(JwtRegisteredClaimNames.Sub)
                               ?? user.FindFirst("uid")
                               ?? user.FindFirst("id");

                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int id))
                {
                    return id;
                }

                return null;
            }
        }

        public string? NationalId =>
            _httpContextAccessor.HttpContext?.User?.FindFirst("national_id")?.Value
            ?? _httpContextAccessor.HttpContext?.User?.FindFirst("NationalId")?.Value;

        public string? Role =>
            _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Role)?.Value
            ?? _httpContextAccessor.HttpContext?.User?.FindFirst("role")?.Value;

        public bool IsAuthenticated =>
            _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;


       
    }
}
