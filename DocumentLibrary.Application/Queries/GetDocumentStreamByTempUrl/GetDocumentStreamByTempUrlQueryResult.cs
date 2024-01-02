namespace DocumentsLibrary.Application.Queries.GetListOfDocuments
{
    public class GetDocumentStreamByTempUrlQueryResult
    {
        public required Stream FileStream { get; set; }
        public required string FileName { get; set; }
        public required string ContentType { get; set; }
    }
}
