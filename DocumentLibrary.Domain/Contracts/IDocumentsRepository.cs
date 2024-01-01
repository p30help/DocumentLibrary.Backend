using DocumentLibrary.Domain.Common;
using DocumentLibrary.Domain.Models;
using DocumentLibrary.Domain.ValueObjects;

namespace DocumentLibrary.Domain.Contracts
{
    public interface IDocumentsRepository
    {
        Task AddDocument(Document document);
        Task<Document?> GetDocument(Guid documentId);
        Task<Document?> GetDocumentByThumbnailId(Guid thumbnailId);
        Task<Document?> GetUserDocument(Guid documentId, Guid userId);
        Task<PagedData<Document>> GetUserDocuments(Guid userId, Pagination pagination);
        Task Update(Document document);
    }
}
