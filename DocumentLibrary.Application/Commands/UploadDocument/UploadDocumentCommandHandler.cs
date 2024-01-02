using DocumentLibrary.Domain.Contracts;
using DocumentLibrary.Domain.Models;
using DocumentsLibrary.Application.Common;
using DocumentsLibrary.Application.Queries.GetListOfDocuments;
using MediatR;

namespace DocumentsLibrary.Application.Commands.UploadDocument;

public class UploadDocumentCommandHandler : IRequestHandler<UploadDocumentCommand, OperationResult<UploadDocumentResult>>
{
    private readonly IDocumentsRepository documentsRepository;
    private readonly IFileRepository fileRepository;
    private readonly IThumbnailGenerator thumbnailGenerator;
    public const int ThumbnailWidth = 300;
    public const int ThumbnailHeight = 300;

    public UploadDocumentCommandHandler(IDocumentsRepository documentsRepository, IFileRepository fileRepository, IThumbnailGenerator thumbnailGenerator)
    {
        this.documentsRepository = documentsRepository;
        this.fileRepository = fileRepository;
        this.thumbnailGenerator = thumbnailGenerator;
    }

    public async Task<OperationResult<UploadDocumentResult>> Handle(UploadDocumentCommand command, CancellationToken cancellationToken)
    {
        var documentId = Guid.NewGuid();

        var document = Document.Create(documentId, command.UserId, command.FileName, command.ContentType);

        await fileRepository.UploadFile(documentId.ToString(), command.ContentType, command.FileStream, DocumentAccessPolicy.Private);

        Guid? thumbnailId = await TryGenerateThumbnail(command, document.GetDocumentType());
        document.SetThumbnailId(thumbnailId);

        await documentsRepository.AddDocument(document);

        return OperationResult<UploadDocumentResult>.Success(new UploadDocumentResult()
        {
            DocumentId = document.Id,
            FileName = command.FileName
        });
    }

    private async Task<Guid?> TryGenerateThumbnail(UploadDocumentCommand command, DocumentType documentType)
    {
        Guid? thumbnailId = Guid.NewGuid();

        if (documentType == DocumentType.Picture)
        {
            using var thumbnailStream = thumbnailGenerator.GenerateImageThumbnail(command.FileStream, command.FileName, ThumbnailWidth, ThumbnailHeight);

            await fileRepository.UploadFile(thumbnailId.Value.ToString(), command.ContentType, thumbnailStream, DocumentAccessPolicy.Public);
        }
        else if (documentType == DocumentType.PDF)
        {
            using var thumbnailStream = thumbnailGenerator.GeneratePdfThumbnail(command.FileStream, command.FileName, ThumbnailWidth, ThumbnailHeight);

            await fileRepository.UploadFile(thumbnailId.Value.ToString(), "image/jpeg", thumbnailStream, DocumentAccessPolicy.Public);
        }
        else
        {
            return null;
        }

        return thumbnailId;
    }
}
