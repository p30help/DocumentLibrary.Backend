using DocumentsLibrary.Application.Contracts;

namespace DocumentLibrary.Infrastructure.Api
{
    public class EndpointUris : IEndpointUris
    {
        private readonly string baseUrl;

        public EndpointUris(string endpointUrl)
        {
            baseUrl = endpointUrl;
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
