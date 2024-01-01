using DocumentsLibrary.Application.Commands.UploadDocument;
using DocumentsLibrary.Application.Common;
using MediatR;

namespace DocumentsLibrary.Application.Queries.GetListOfDocuments
{
    public class GenerateTemporaryLinkCommand : IRequest<OperationResult<GenerateTemporaryLinkResult>>
    {
        public Guid UserId { get; set; }

        public Guid DocumentId { get; set; }

        public int ExpirationTime { get; set; }
    }
}
