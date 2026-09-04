using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace UniversityApp.Application.Features.RequestNotes.Queries.GetRequestNotes
{
    public class GetRequestNotesQuery:IRequest<List<RequestNoteDto>>
    {
        public int RequestId { get; set; }
        public GetRequestNotesQuery(int requestId)
        {
            RequestId = requestId;
        }
    }
}
