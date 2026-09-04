using MediatR;
using UniversityApp.Domain.Enums;

namespace UniversityApp.Application.Common.Events
{
    public class RequestStatusChangedEvent : INotification
    {
        public int RequestId { get; set; }
        public int StudentId { get; set; }
        public RequestStatus NewStatus { get; set; }

        public RequestStatusChangedEvent(int requestId, int studentId, RequestStatus newStatus)
        {
            RequestId = requestId;
            StudentId = studentId;
            NewStatus = newStatus;
        }
    }
}