using DocumentsLibrary.Application.Commands.UploadDocument;
using DocumentsLibrary.Application.Common;
using MediatR;

namespace DocumentsLibrary.Application.Queries.GetListOfDocuments
{
    public class GetUserQuery : IRequest<OperationResult<GetUserResult>>
    {
        public Guid UserId { get; set; }
    }
}
