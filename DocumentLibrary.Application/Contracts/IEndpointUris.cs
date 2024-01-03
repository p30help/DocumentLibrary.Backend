namespace DocumentsLibrary.Application.Contracts
{
    public interface IEndpointUris
    {
        Uri GetDocumentByTempLink(string encryptedText);
        Uri GetDocumentUrl(Guid documentId);
    }
}
