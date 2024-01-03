namespace DocumentsLibrary.Application.Contracts
{
    public interface IThumbnailGenerator
    {
        Stream GenerateImageThumbnail(Stream fileStream, string imageFileName, int width, int height);

        Stream GeneratePdfThumbnail(Stream fileStream, string pdfFileName, int width, int height);
    }
}
