using System;

namespace UniversityApp.Application.Features.RequestNotes.Queries.GetRequestNotes
{
    public class RequestNoteDto
    {
        public int Id { get; set; }
        public string? Content { get; set; }
        public string? FileUrl { get; set; }
        public string SenderRole { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}