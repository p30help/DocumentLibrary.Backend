using DocumentsLibrary.Application.Common;
using System.Drawing;

namespace DocumentLibrary.Infrastructure.Thumbnail
{
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
            //resized.Save(imageStream, ImageFormat.Jpeg);
            //var imageBytes = imageStream.ToArray();

            imageStream.Position = 0;
            return imageStream;

            //Image img = Image.FromStream(fileStream);
            //
            //img = ChangeImageSize(img, width, height);
            //
            //var ext = Path.GetExtension(imageFileName).ToLower();
            //
            //byte[] finalImgArray;
            //using (var ms = new MemoryStream())
            //{
            //    if (ext == ".gif")                
            //        img.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            //    else if (ext == ".jpg" || ext == ".jpeg")                
            //        img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);                
            //    else if (ext == ".png")                
            //        img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);                
            //    else if (ext == ".bmp")                
            //        img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);                
            //    else                
            //        throw new Exception("image format not recognized");                
            //
            //    finalImgArray = ms.ToArray();
            //
            //    return ms;
            //}
            //
            //
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

        //public static System.Drawing.Image ChangeImageSize(System.Drawing.Image image, int width, int height)
        //{
        //    #region calculations
        //
        //    int sourceWidth = image.Width;
        //    int sourceHeight = image.Height;
        //    int sourceX = 0;
        //    int sourceY = 0;
        //    double destX = 0;
        //    double destY = 0;
        //
        //    double nScale = 0;
        //    double nScaleW = 0;
        //    double nScaleH = 0;
        //
        //    nScaleW = ((double)width / (double)sourceWidth);
        //    nScaleH = ((double)height / (double)sourceHeight);
        //    nScale = Math.Max(nScaleH, nScaleW);
        //    destY = (height - sourceHeight * nScale) / 2;
        //    destX = (width - sourceWidth * nScale) / 2;
        //
        //    if (nScale > 1)
        //        nScale = 1;
        //
        //    int destWidth = (int)Math.Round(sourceWidth * nScale);
        //    int destHeight = (int)Math.Round(sourceHeight * nScale);
        //
        //    #endregion
        //
        //    System.Drawing.Bitmap bmPhoto = null;
        //
        //    try
        //    {
        //        bmPhoto = new System.Drawing.Bitmap(destWidth + (int)Math.Round(2 * destX), destHeight + (int)Math.Round(2 * destY));
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new ApplicationException(string.Format("destWidth:{0}, destX:{1}, destHeight:{2}, desxtY:{3}, Width:{4}, Height:{5}",
        //            destWidth, destX, destHeight, destY, width, height), ex);
        //    }
        //
        //    using (System.Drawing.Graphics grPhoto = System.Drawing.Graphics.FromImage(bmPhoto))
        //    {
        //        grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
        //        grPhoto.CompositingQuality = CompositingQuality.HighQuality;
        //        grPhoto.SmoothingMode = SmoothingMode.HighQuality;
        //
        //        Rectangle to = new System.Drawing.Rectangle((int)Math.Round(destX), (int)Math.Round(destY), destWidth, destHeight);
        //        Rectangle from = new System.Drawing.Rectangle(sourceX, sourceY, sourceWidth, sourceHeight);
        //        grPhoto.DrawImage(image, to, from, System.Drawing.GraphicsUnit.Pixel);
        //
        //        return bmPhoto;
        //    }
        //}
    }
}
