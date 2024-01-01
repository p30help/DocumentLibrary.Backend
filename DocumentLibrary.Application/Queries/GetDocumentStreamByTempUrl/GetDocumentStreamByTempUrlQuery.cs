using DocumentsLibrary.Application.Common;
using MediatR;

namespace DocumentsLibrary.Application.Queries.GetListOfDocuments
{
    public class GetDocumentStreamByTempUrlQuery : IRequest<OperationResult<GetDocumentStreamByTempUrlQueryResult>>
    {
        public string EncryptedText { get; set; }
    }
}
