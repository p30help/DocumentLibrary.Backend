using DocumentLibrary.Domain.Models;

namespace DocumentsLibrary.Application.Queries.GetListOfDocuments
{
    public class GetListOfDocumentsResult
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public DateTime RecordDate { get; set; }
        public string DocumentType { get; set; }
        public int DownloadCount { get; set; }
        //public string DocumentUrl { get; set; }
        public string? ThumbnailUrl { get; set; }
    }
}
