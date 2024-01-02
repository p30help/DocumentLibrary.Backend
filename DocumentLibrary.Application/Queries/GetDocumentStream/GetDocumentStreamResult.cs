namespace DocumentsLibrary.Application.Queries.GetListOfDocuments
{
    public class GetDocumentStreamResult
    {
        public required Stream FileStream { get; set; }
        public required string FileName { get; set; }
        public required string ContentType { get; set; }
    }
}
