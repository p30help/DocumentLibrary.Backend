using DocumentLibrary.Domain.Contracts;
using DocumentsLibrary.Application.Common;
using DocumentsLibrary.Application.Queries.GetListOfDocuments;
using MediatR;

namespace DocumentsLibrary.Application.Queries.GetDocumentStream
{
    public class GetDocumentStreamQueryHandler : IRequestHandler<GetDocumentStreamQuery, OperationResult<GetDocumentStreamResult>>
    {
        private readonly IDocumentsRepository documentsRepository;
        private readonly IFileRepository fileRepository;

        public GetDocumentStreamQueryHandler(IDocumentsRepository documentsRepository, IFileRepository fileRepository)
        {
            this.documentsRepository = documentsRepository;
            this.fileRepository = fileRepository;
        }

        public async Task<OperationResult<GetDocumentStreamResult>> Handle(GetDocumentStreamQuery query, CancellationToken cancellationToken)
        {
            var document = await documentsRepository.GetUserDocument(query.DocumentId, query.UserId);
            if (document == null)
                return OperationResult<GetDocumentStreamResult>.NotFound("Document not found");

            document.IncreasDownloadCount();
            await documentsRepository.Update(document);

            var fileStream = await fileRepository.GetFileStream(document!.Id, DocumentAccessPolicy.Private);

            return OperationResult<GetDocumentStreamResult>.Success(new GetDocumentStreamResult()
            {
                FileStream = fileStream,
                ContentType = document.ContentType,
                FileName = document.FileName,
            });
        }

    }
}
