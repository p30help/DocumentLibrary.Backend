namespace DocumentLibrary.Domain.Contracts
{
    public interface IFileRepository
    {
        public Task UploadFile(string fileNameWithExtention, string contentType, Stream fileStreamData, DocumentAccessPolicy accessPolicy);

        Task<Stream> GetFileStream(Guid fileId, DocumentAccessPolicy accessPolicy);

        string GetDirectFileUrl(Guid fileId, DocumentAccessPolicy accessPolicy);
    }

    public enum DocumentAccessPolicy
    {
        Private,
        Public
    }
}
