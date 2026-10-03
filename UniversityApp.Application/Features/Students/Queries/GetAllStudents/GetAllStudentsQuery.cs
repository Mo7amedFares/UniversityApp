using MediatR;
using UniversityApp.Application.Common.Models;

namespace UniversityApp.Application.Features.Students.Queries.GetAllStudents
{
    public class GetAllStudentsQuery : IRequest<PagedResult<StudentDto>>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 50;
        public string? Search { get; set; }
    }
}