using DocumentLibrary.Domain.Common;
using DocumentLibrary.Domain.Contracts;
using DocumentLibrary.Domain.ValueObjects;
using DocumentsLibrary.Application.Common;
using MediatR;

namespace DocumentsLibrary.Application.Queries.GetListOfDocuments
{
    public class GetListOfDocumentsQueryHandler : IRequestHandler<GetListOfDocumentsQuery, OperationResult<PagedData<GetListOfDocumentsResult>>>
    {
        private readonly IDocumentsRepository documentsRepository;
        private readonly IFileRepository fileRepository;

        public GetListOfDocumentsQueryHandler(IDocumentsRepository documentsRepository, IFileRepository fileRepository)
        {
            this.documentsRepository = documentsRepository;
            this.fileRepository = fileRepository;
        }

        public async Task<OperationResult<PagedData<GetListOfDocumentsResult>>> Handle(GetListOfDocumentsQuery query, CancellationToken cancellationToken)
        {
            var documentsRaw = await documentsRepository.GetUserDocuments(query.UserId, new Pagination(query.PageNumber ?? 1, query.PageSize ?? 50));

            var documents = new List<GetListOfDocumentsResult>();
            foreach (var item in documentsRaw.Data)
            {
                documents.Add(new GetListOfDocumentsResult()
                {
                    ContentType = item.ContentType,
                    FileName = item.FileName,
                    Id = item.Id,
                    RecordDate = item.RecordDate.LocalDateTime,
                    DocumentType = item.GetDocumentType().ToString().ToLower(),
                    DownloadCount = item.DownloadCount,
                    ThumbnailUrl = item.ThumbnailId.HasValue ? fileRepository.GetDirectFileUrl(item.ThumbnailId.Value, DocumentAccessPolicy.Public) : null,
                });
            }

            var result = new PagedData<GetListOfDocumentsResult>(documents, documentsRaw.CurrentPageNumber, documentsRaw.TotalCount);

            return OperationResult<PagedData<GetListOfDocumentsResult>>.Success(result);
        }

    }
}
