using DocumentsLibrary.Application.Common;
using MediatR;

namespace DocumentsLibrary.Application.Queries.GetListOfDocuments
{
    public class GetDocumentStreamQuery : IRequest<OperationResult<GetDocumentStreamResult>>
    {
        public required Guid DocumentId { get; set; }
        public required Guid UserId { get; set; }
    }
}
