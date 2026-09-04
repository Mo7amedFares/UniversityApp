using UniversityApp.Domain.Enums;

namespace UniversityApp.Application.Features.Requests.Queries.GetMyRequests
{
    public class RequestDto
    {
        public int Id { get; set; }
        public string ServiceTitle { get; set; } = string.Empty;
        public RequestStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}