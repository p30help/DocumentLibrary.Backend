using DocumentsLibrary.Application.Commands.UploadDocument;
using DocumentsLibrary.Application.Common;
using MediatR;

namespace DocumentsLibrary.Application.Queries.GetListOfDocuments
{
    public class UploadDocumentCommand : IRequest<OperationResult<UploadDocumentResult>>
    {
        public required Guid UserId { get; set; }

        public required string FileName { get; set; }

        public required string ContentType { get; set; }

        public required Stream FileStream { get; set; }
    }
}
