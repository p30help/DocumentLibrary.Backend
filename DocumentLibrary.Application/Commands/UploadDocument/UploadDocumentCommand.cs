using DocumentsLibrary.Application.Commands.UploadDocument;
using DocumentsLibrary.Application.Common;
using MediatR;

namespace DocumentsLibrary.Application.Queries.GetListOfDocuments
{
    public class UploadDocumentCommand : IRequest<OperationResult<UploadDocumentResult>>
    {
        public Guid UserId { get; set; }

        public string FileName { get; set; }

        public string ContentType { get; set; }

        public Stream FileStream { get; set; }
    }
}
