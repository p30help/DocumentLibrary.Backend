using DocumentLibrary.Domain.Common;
using DocumentLibrary.Domain.Contracts;
using DocumentLibrary.Domain.Models;
using DocumentLibrary.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace DocumentLibrary.Infrastructure.EF.Repositories;

public class DocumentsRepository : IDocumentsRepository
{
    private readonly AppDbContext dbContext;

    public DocumentsRepository(AppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task AddDocument(Document document)
    {
        dbContext.Documents.Add(document);
        await dbContext.SaveChangesAsync();
    }

    public async Task Update(Document document)
    {
        dbContext.Documents.Update(document);
        await dbContext.SaveChangesAsync();
    }

    public async Task<Document?> GetDocument(Guid documentId)
    {
        return await dbContext.Documents.FirstOrDefaultAsync(x => x.Id == documentId);
    }

    public async Task<Document?> GetUserDocument(Guid documentId, Guid userId)
    {
        return await dbContext.Documents.FirstOrDefaultAsync(x => x.UserId == userId && x.Id == documentId);
    }

    public async Task<Document?> GetDocumentByThumbnailId(Guid thumbnailId)
    {
        return await dbContext.Documents.FirstOrDefaultAsync(x => x.ThumbnailId == thumbnailId);
    }

    public async Task<PagedData<Document>> GetUserDocuments(Guid userId, Pagination pagination)
    {
        var query = dbContext.Documents.Where(x => x.UserId == userId);

        var totalCount = await query.CountAsync();
        var skip = (pagination.PageNumber - 1) * pagination.PageSize;

        var docs = await query.OrderByDescending(x => x.RecordDate)
            .OrderByDescending(x => x.RecordDate)
            .Skip(skip)
            .Take(pagination.PageSize)
            .ToListAsync();

        return new PagedData<Document>(docs, pagination.PageNumber, totalCount);
    }
}
