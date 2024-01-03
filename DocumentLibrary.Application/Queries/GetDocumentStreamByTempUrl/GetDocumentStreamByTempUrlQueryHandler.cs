using DocumentLibrary.Domain.Contracts;
using DocumentLibrary.Domain.Models;
using DocumentsLibrary.Application.Common;
using DocumentsLibrary.Application.Contracts;
using DocumentsLibrary.Application.Queries.GetListOfDocuments;
using MediatR;

namespace DocumentsLibrary.Application.Queries.GetDocumentStreamByTempUrl;

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

        var fileStreamTask = fileRepository.GetFileStream(document!.Id, DocumentAccessPolicy.Private);

        var increaseCountTask = IncreaseDownloadCount(document);

        await Task.WhenAll(fileStreamTask, increaseCountTask);

        return OperationResult<GetDocumentStreamByTempUrlQueryResult>.Success(new GetDocumentStreamByTempUrlQueryResult()
        {
            FileStream = await fileStreamTask,
            ContentType = document.ContentType,
            FileName = document.FileName,
        });
    }

    private async Task IncreaseDownloadCount(Document document)
    {
        document.IncreasDownloadCount();
        await documentsRepository.Update(document);
    }

}
