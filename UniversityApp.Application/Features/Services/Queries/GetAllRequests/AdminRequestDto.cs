using Azure.Core;
using UniversityApp.Domain.Enums;

namespace UniversityApp.Application.Features.Requests.Queries.GetAllRequests
{
    public class AdminRequestDto
    {
        public int RequestId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string NationalId { get; set; } = string.Empty;
        public string ServiceTitle { get; set; } = string.Empty;
        public RequestStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}