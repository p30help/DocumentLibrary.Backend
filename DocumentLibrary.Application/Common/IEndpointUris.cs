namespace DocumentsLibrary.Application.Common
{
    public interface IEndpointUris
    {
        Uri GetDocumentByTempLink(string encryptedText);
        Uri GetDocumentUrl(Guid documentId);
    }
}
