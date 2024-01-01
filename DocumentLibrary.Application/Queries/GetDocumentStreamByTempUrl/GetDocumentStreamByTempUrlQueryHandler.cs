using DocumentLibrary.Domain.Contracts;
using DocumentsLibrary.Application.Common;
using DocumentsLibrary.Application.Queries.GetListOfDocuments;
using MediatR;

namespace DocumentsLibrary.Application.Queries.GetDocumentStreamByTempUrl
{
    public class GetDocumentStreamByTempUrlQueryHandler : IRequestHandler<GetDocumentStreamByTempUrlQuery, OperationResult<GetDocumentStreamByTempUrlQueryResult>>
    {
        private readonly IDocumentsRepository documentsRepository;
        private readonly ITempCodeGenerator tempLinkGenerator;
        private readonly IFileRepository fileRepository;

        public GetDocumentStreamByTempUrlQueryHandler(IDocumentsRepository documentsRepository, ITempCodeGenerator tempLinkGenerator, IFileRepository fileRepository)
        {
            this.documentsRepository = documentsRepository;
            this.tempLinkGenerator = tempLinkGenerator;
            this.fileRepository = fileRepository;
        }

        public async Task<OperationResult<GetDocumentStreamByTempUrlQueryResult>> Handle(GetDocumentStreamByTempUrlQuery query, CancellationToken cancellationToken)
        {
            var tempLinkValidation = tempLinkGenerator.ValidateTempCode(query.EncryptedText);
            if (tempLinkValidation.IsValid == false)
            {
                return OperationResult<GetDocumentStreamByTempUrlQueryResult>.NotFound("Document not found");
            }

            var document = await documentsRepository.GetDocument(tempLinkValidation.DocumentId!.Value);
            if (document == null)
            {
                return OperationResult<GetDocumentStreamByTempUrlQueryResult>.NotFound("Document not found");
            }

            document.IncreasDownloadCount();
            await documentsRepository.Update(document);

            var fileStream = await fileRepository.GetFileStream(document!.Id, DocumentAccessPolicy.Private);

            return OperationResult<GetDocumentStreamByTempUrlQueryResult>.Success(new GetDocumentStreamByTempUrlQueryResult()
            {
                FileStream = fileStream,
                ContentType = document.ContentType,
                FileName = document.FileName,
            });
        }

    }
}
