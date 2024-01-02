namespace DocumentsLibrary.Application.Commands.UploadDocument
{
    public class GenerateTemporaryLinkResult
    {
        public required string Url { get; set; }
        public required string ContentType { get; set; }
        public required string FileName { get; set; }
    }
}
