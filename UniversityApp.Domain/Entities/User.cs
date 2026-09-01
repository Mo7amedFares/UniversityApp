using System;
using System.Collections.Generic;
using System.Text;
using UniversityApp.Domain.Common;
using UniversityApp.Domain.Enums;

namespace UniversityApp.Domain.Entities
{
    public class User:BaseEntity
    {
        public string NationalId { get; set; } = string.Empty;
        public string? StudentCode { get; set; }
        public string Name { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public UserRole Role { get; set; }

        public ICollection<Request> Requests { get; set; } = new List<Request>();
        public ICollection<Notfication> Notifications { get; set; } = new List<Notfication>();
    }
}
