using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using UniversityApp.Domain.Common;

namespace UniversityApp.Domain.Entities
{
    public class Notfication: BaseEntity
    {
        public int UserId { get; set; }
        public int? RequestId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; } = string.Empty;
        public bool IsRead { get; set; } = false;
        public User User { get; set; }  = null!; // Navigation property to the User entity
        public Request? Request { get; set; } // Navigation property to the Request entity

    }
}
