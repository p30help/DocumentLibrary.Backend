namespace DocumentsLibrary.Application.Commands.UploadDocument
{
    public class UploadDocumentResult
    {
        public required Guid DocumentId { get; set; }
        public required string FileName { get; set; }
    }
}
