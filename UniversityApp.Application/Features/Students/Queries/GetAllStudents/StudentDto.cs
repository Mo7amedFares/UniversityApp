using System;
using System.Collections.Generic;
using System.Text;

namespace UniversityApp.Application.Features.Students.Queries.GetAllStudents
{
    public class StudentDto
    {
        public int Id { get; set; }
        public string NationalId { get; set; }
        public string PhoneNumber { get; set; }
        public string StudentCode { get; set; }
        public string Name { get; set; }
        public int Role { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
