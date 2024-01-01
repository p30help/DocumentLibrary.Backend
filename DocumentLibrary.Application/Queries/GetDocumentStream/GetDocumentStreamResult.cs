namespace DocumentsLibrary.Application.Queries.GetListOfDocuments
{
    public class GetDocumentStreamResult
    {
        public Stream FileStream { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
    }
}
