namespace UniversityApp.Application.Common.Interfaces
{
    public interface ICurrentUserService
    {
        int? UserId { get; }
        string? NationalId { get; }
        string? Role { get; }
        bool IsAuthenticated { get; }
    }
}