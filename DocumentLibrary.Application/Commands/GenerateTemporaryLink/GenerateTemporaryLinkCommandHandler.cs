using DocumentLibrary.Domain.Contracts;
using DocumentsLibrary.Application.Commands.UploadDocument;
using DocumentsLibrary.Application.Common;
using DocumentsLibrary.Application.Queries.GetListOfDocuments;
using MediatR;

namespace DocumentsLibrary.Application.Commands.GenerateTemporaryLink;

public class GenerateTemporaryLinkCommandHandler : IRequestHandler<GenerateTemporaryLinkCommand, OperationResult<GenerateTemporaryLinkResult>>
{
    private readonly IDocumentsRepository documentsRepository;
    private readonly ITempCodeGenerator tempLinkGenerator;
    private readonly IEndpointUris endpointUrls;

    public GenerateTemporaryLinkCommandHandler(IDocumentsRepository documentsRepository, ITempCodeGenerator tempLinkGenerator, IEndpointUris endpointUrls)
    {
        this.documentsRepository = documentsRepository;
        this.tempLinkGenerator = tempLinkGenerator;
        this.endpointUrls = endpointUrls;
    }

    public async Task<OperationResult<GenerateTemporaryLinkResult>> Handle(GenerateTemporaryLinkCommand command, CancellationToken cancellationToken)
    {
        var document = await documentsRepository.GetUserDocument(command.DocumentId, command.UserId);

        if (document == null)
            return OperationResult<GenerateTemporaryLinkResult>.NotFound("Document not found");

        if (command.ExpirationTime < 5)
            return OperationResult<GenerateTemporaryLinkResult>.Failure("Expiration time must be greater than 5 minutes");

        var encryptedText = tempLinkGenerator.GenerateTempCode(document.Id, TimeSpan.FromMinutes(command.ExpirationTime));

        if (string.IsNullOrWhiteSpace(encryptedText))
            return OperationResult<GenerateTemporaryLinkResult>.Failure("Generating temporary link faced with the problem");

        return OperationResult<GenerateTemporaryLinkResult>.Success(new GenerateTemporaryLinkResult()
        {
            Url = endpointUrls.GetDocumentByTempLink(encryptedText).ToString(),
            FileName = document.FileName,
            ContentType = document.ContentType
        });
    }

}
