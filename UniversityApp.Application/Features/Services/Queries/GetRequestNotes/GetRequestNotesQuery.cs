using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using UniversityApp.Application.Features.RequestNotes.Queries.GetRequestNotes;

namespace UniversityApp.Application.Features.Services.Queries.GetRequestNotes
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
