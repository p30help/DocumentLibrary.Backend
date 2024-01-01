using DocumentLibrary.Domain.Common;
using DocumentsLibrary.Application.Common;
using MediatR;

namespace DocumentsLibrary.Application.Queries.GetListOfDocuments
{
    public class GetListOfDocumentsQuery : IRequest<OperationResult<PagedData<GetListOfDocumentsResult>>>
    {
        public Guid UserId { get; set; }
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
    }
}
