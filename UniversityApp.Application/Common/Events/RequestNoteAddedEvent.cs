using MediatR;

namespace UniversityApp.Application.Common.Events
{
    public class RequestNoteAddedEvent : INotification
    {
        public int RequestId { get; set; }
        public int StudentId { get; set; }

        public RequestNoteAddedEvent(int requestId, int studentId)
        {
            RequestId = requestId;
            StudentId = studentId;
        }
    }
}