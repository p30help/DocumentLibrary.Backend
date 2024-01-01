using DocumentsLibrary.Application.Common;
using MediatR;

namespace DocumentsLibrary.Application.Queries.GetListOfDocuments
{
    public class GetDocumentStreamQuery : IRequest<OperationResult<GetDocumentStreamResult>>
    {
        public Guid DocumentId { get; set; }
        public Guid UserId { get; set; }
    }
}
