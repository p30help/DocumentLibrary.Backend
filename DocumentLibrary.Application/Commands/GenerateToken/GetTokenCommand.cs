using DocumentsLibrary.Application.Commands.UploadDocument;
using DocumentsLibrary.Application.Common;
using MediatR;

namespace DocumentsLibrary.Application.Queries.GetListOfDocuments
{
    public class GetDocumentUrlQuery : IRequest<OperationResult<GetTokenResult>>
    {
        public required string Email { get; set; }

        public required string Password { get; set; }
    }
}
