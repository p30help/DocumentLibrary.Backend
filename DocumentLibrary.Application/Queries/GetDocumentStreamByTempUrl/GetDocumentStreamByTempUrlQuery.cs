using DocumentsLibrary.Application.Common;
using MediatR;

namespace DocumentsLibrary.Application.Queries.GetListOfDocuments
{
    public class GetDocumentStreamByTempUrlQuery : IRequest<OperationResult<GetDocumentStreamByTempUrlQueryResult>>
    {
        public required string EncryptedText { get; set; }
    }
}
