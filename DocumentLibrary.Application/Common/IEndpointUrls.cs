namespace DocumentsLibrary.Application.Common
{
    public interface IEndpointUrls
    {
        Uri GetDocumentByTempLink(string encryptedText);
        Uri GetDocumentUrl(Guid documentId);
        Uri GetThumbnailUrl(Guid thumbnailId);
    }
}
