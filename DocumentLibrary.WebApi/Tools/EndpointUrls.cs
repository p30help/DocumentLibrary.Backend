using DocumentsLibrary.Application.Common;

namespace DocumentsLibrary.Application
{
    public class EndpointUrls : IEndpointUrls
    {
        private readonly string baseUrl;

        public EndpointUrls(string endpointUrl)
        {
            this.baseUrl = endpointUrl;
        }

        public Uri GetDocumentByTempLink(string encryptedText)
        {
            return new Uri($"{baseUrl}/api/documents/temp/{encryptedText}");
        }

        public Uri GetDocumentUrl(Guid documentId)
        {
            return new Uri($"{baseUrl}/api/documents/{documentId}");
        }
    }
}
