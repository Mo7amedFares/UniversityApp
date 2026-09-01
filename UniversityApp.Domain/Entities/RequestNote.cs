using System;
using UniversityApp.Domain.Common;
using UniversityApp.Domain.Enums;

namespace UniversityApp.Domain.Entities
{
    public class RequestNote : BaseEntity
    {
        public int RequestId { get; set; }
        public Request Request { get; set; } = null!; // Navigation property

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public UserRole SenderRole { get; set; }

        
        public string? Content { get; set; }

        public string? FileUrl { get; set; }
    }
}