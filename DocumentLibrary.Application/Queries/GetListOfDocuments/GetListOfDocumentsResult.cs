namespace DocumentsLibrary.Application.Queries.GetListOfDocuments
{
    public class GetListOfDocumentsResult
    {
        public required Guid Id { get; set; }
        public required string FileName { get; set; }
        public required string ContentType { get; set; }
        public required DateTime RecordDate { get; set; }
        public required string DocumentType { get; set; }
        public required int DownloadCount { get; set; }
        public string? ThumbnailUrl { get; set; }
    }
}
