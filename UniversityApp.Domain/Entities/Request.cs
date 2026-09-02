using System;
using System.Collections.Generic;
using System.Text;
using UniversityApp.Domain.Common;
using UniversityApp.Domain.Enums;

namespace UniversityApp.Domain.Entities
{
    public class Request:BaseEntity
    {
        public int ServiceId { get; set; }
        public int StudentId { get; set; }
        public RequestStatus Status { get; set; } = RequestStatus.Pending;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public Service Service { get; set; } = null!; // Navigation property to the Service entity
        public User Student { get; set; } = null!; // Navigation property to the Student entity
        public Payment? Payment { get; set; } // Navigation property to the Payment entity
        public ICollection<RequestNote> RequestNotes { get; set; } = new List<RequestNote>();
        public ICollection<Notfication> Notifications { get; set; } = new List<Notfication>();


    }
}
