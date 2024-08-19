using QuizApp_API.BusinessLogic.Interfaces;
using System.Drawing.Imaging;
using System.Drawing;

namespace QuizApp_API.BusinessLogic.Services
{
    public class ImageConverter : IImageConverter
    {
        public byte[] ResizeImage(byte[] bytes, int width, int height)
        {
            using (var inputStream = new MemoryStream(bytes))
            using (var image = Image.FromStream(inputStream))
            {
                // Create a new bitmap with the desired dimensions
                using (var resizedImage = new Bitmap(width, height))
                {
                    // Use Graphics to draw the resized image
                    using (var graphics = Graphics.FromImage(resizedImage))
                    {
                        graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                        graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                        // Draw the original image onto the new bitmap
                        graphics.DrawImage(image, 0, 0, width, height);

                        // Save the resized image to a memory stream
                        using (var outputStream = new MemoryStream())
                        {
                            resizedImage.Save(outputStream, ImageFormat.Jpeg);

                            // Return the byte array of the resized image
                            return outputStream.ToArray();
                        }
                    }
                }
            }
        }
    }
}
