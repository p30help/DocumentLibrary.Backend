using DocumentLibrary.Domain.Common.Exceptions;
using DocumentLibrary.Domain.Users;

namespace DocumentLibrary.Domain.Models
{
    public class Document
    {
        public Guid Id { get; private set; }
        public Guid? ThumbnailId { get; private set; }
        public string FileName { get; private set; }
        public string ContentType { get; private set; }
        public DateTimeOffset RecordDate { get; private set; }
        public int DownloadCount { get; private set; }
        public Guid UserId { get; private set; }
        public AppUser User { get; private set; }

        private Document() { }

        public static Document Create(Guid id, Guid userId, string fileName, string contentType)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new DomainStateException("File name must be defined");
            }

            if (Path.GetExtension(fileName).Length <= 1)
            {
                throw new DomainStateException("File must have extension");
            }

            if (string.IsNullOrWhiteSpace(contentType))
            {
                throw new DomainStateException("ContentType must be defined");
            }

            if (userId == Guid.Empty)
            {
                throw new DomainStateException("User Id must be defined");
            }

            if (id == Guid.Empty)
            {
                throw new DomainStateException("Document Id must be defined");
            }

            if (DetectDocumentType(fileName) == DocumentType.Unknown)
            {
                throw new DomainStateException("Document type is not allowed. Just picture, pdf, word, text, excel are allowed");
            }

            return new Document()
            {
                Id = id,
                DownloadCount = 0,
                FileName = fileName,
                ContentType = contentType,
                RecordDate = DateTimeOffset.UtcNow,
                UserId = userId
            };
        }

        public void SetThumbnailId(Guid? thumbnailId)
        {
            this.ThumbnailId = thumbnailId;
        }

        public void IncreasDownloadCount()
        {
            DownloadCount++;
        }

        public DocumentType GetDocumentType()
        {
            return DetectDocumentType(FileName);
        }

        private static DocumentType DetectDocumentType(string fileName)
        {
            var fileExt = Path.GetExtension(fileName);
            switch (fileExt.ToLower())
            {
                case ".pdf":
                    return DocumentType.PDF;
                case ".txt":
                    return DocumentType.Txt;
                case ".docx" or ".doc":
                    return DocumentType.Word;
                case ".xlsx" or ".xls":
                    return DocumentType.Excel;
                case ".jpg" or ".jpeg" or ".png" or ".gif":
                    return DocumentType.Picture;
            }

            return DocumentType.Unknown;
        }
    }
}
