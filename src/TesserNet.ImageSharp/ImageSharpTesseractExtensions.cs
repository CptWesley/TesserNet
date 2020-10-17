using System;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace TesserNet
{
    /// <summary>
    /// Provides extension methods for the <see cref="Tesseract"/> class.
    /// </summary>
    public static class ImageSharpTesseractExtensions
    {
        /// <summary>
        /// Performs OCR on the given image.
        /// </summary>
        /// <param name="tesseract">The tesseract instance.</param>
        /// <param name="image">The image.</param>
        /// <returns>The found text as a UTF8 string.</returns>
        public static string Read(this Tesseract tesseract, Image image)
            => tesseract.Read(image, new Rectangle(-1, -1, -1, -1));

        /// <summary>
        /// Performs OCR on a rectangle inside the given image.
        /// </summary>
        /// <param name="tesseract">The tesseract instance.</param>
        /// <param name="image">The image.</param>
        /// <param name="rectangle">The rectangle to perform OCR in.</param>
        /// <returns>The found text as a UTF8 string.</returns>
        public static string Read(this Tesseract tesseract, Image image, Rectangle rectangle)
        {
            if (tesseract is null)
            {
                throw new ArgumentNullException(nameof(tesseract));
            }

            if (image is null)
            {
                throw new ArgumentNullException(nameof(image));
            }

            using (Image<Rgba32> bmp = image.CloneAs<Rgba32>())
            {
                byte[] data = BitmapToBytes(bmp);
                return tesseract.Read(data, bmp.Width, bmp.Height, 4, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
            }
        }

        private static byte[] BitmapToBytes(Image<Rgba32> bmp)
        {
            byte[] bytes = new byte[bmp.Width * bmp.Height * 4];
            int index = 0;

            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    Rgba32 z = bmp[x, y];
                    bytes[index] = z.A;
                    bytes[index + 1] = z.B;
                    bytes[index + 2] = z.G;
                    bytes[index + 3] = z.R;
                    index += 4;
                }
            }

            return bytes;
        }
    }
}
