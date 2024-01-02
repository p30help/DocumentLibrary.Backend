using DocumentsLibrary.Application.Common;
using System.Drawing;

namespace DocumentLibrary.Infrastructure.Thumbnail;

public class ThumbnailGenerator : IThumbnailGenerator
{
    public Stream GeneratePdfThumbnail(Stream fileStream, string pdfFileName, int width, int height)
    {
        string tempPath = System.IO.Path.GetTempPath();
        var pdfFileUrl = $@"{tempPath}{Guid.NewGuid()}.pdf";
        var thumbFileUrl = $@"{tempPath}{Guid.NewGuid()}.jpg";

        fileStream.Position = 0;
        using (FileStream file = new FileStream(pdfFileUrl, FileMode.Create, System.IO.FileAccess.Write))
        {
            byte[] bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, (int)fileStream.Length);
            file.Write(bytes, 0, bytes.Length);
            fileStream.Close();
        }

        GhostscriptSharp.GhostscriptWrapper.GeneratePageThumb(pdfFileUrl, thumbFileUrl, 1, 80, 80, width, height);

        var thumbnailStream = new MemoryStream();
        using (FileStream file = new FileStream(thumbFileUrl, FileMode.Open, FileAccess.Read))
            file.CopyTo(thumbnailStream);

        thumbnailStream.Position = 0;
        return thumbnailStream;
    }

    public Stream GenerateImageThumbnail(Stream fileStream, string imageFileName, int width, int height)
    {

        var image = Image.FromStream(fileStream);
        var resized = new Bitmap(image, new Size(width, height));
        var imageStream = new MemoryStream();
        resized.Save(imageStream, GetImageFormat(imageFileName));

        imageStream.Position = 0;
        return imageStream;

    }

    private System.Drawing.Imaging.ImageFormat GetImageFormat(string imageFileName)
    {
        var ext = Path.GetExtension(imageFileName).ToLower();
        if (ext == ".gif")
            return System.Drawing.Imaging.ImageFormat.Gif;
        else if (ext == ".jpg" || ext == ".jpeg")
            return System.Drawing.Imaging.ImageFormat.Jpeg;
        else if (ext == ".png")
            return System.Drawing.Imaging.ImageFormat.Png;
        else if (ext == ".bmp")
            return System.Drawing.Imaging.ImageFormat.Bmp;
        else
            throw new Exception("image format not recognized");
    }
}
