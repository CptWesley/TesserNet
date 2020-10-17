using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
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

        /// <summary>
        /// Performs OCR on the given image.
        /// </summary>
        /// <param name="tesseract">The tesseract instance.</param>
        /// <param name="image">The image.</param>
        /// <returns>The found text as a UTF8 string.</returns>
        public static Task<string> ReadAsync(this Tesseract tesseract, Image image)
            => Task.Run(() => tesseract.Read(image));

        /// <summary>
        /// Performs OCR on a rectangle inside the given image.
        /// </summary>
        /// <param name="tesseract">The tesseract instance.</param>
        /// <param name="image">The image.</param>
        /// <param name="rectangle">The rectangle to perform OCR in.</param>
        /// <returns>The found text as a UTF8 string.</returns>
        public static Task<string> ReadAsync(this Tesseract tesseract, Image image, Rectangle rectangle)
            => Task.Run(() => tesseract.Read(image, rectangle));

        /// <summary>
        /// Performs OCR on a rectangle inside the given image.
        /// </summary>
        /// <param name="tesseract">The tesseract instance.</param>
        /// <param name="image">The image.</param>
        /// <returns>The found text as a UTF8 string.</returns>
        public static Task<string> ReadAsync(this TesseractPool tesseract, Image image)
            => tesseract.ReadAsync(image, new Rectangle(-1, -1, -1, -1));

        /// <summary>
        /// Performs OCR on a rectangle inside the given image.
        /// </summary>
        /// <param name="tesseract">The tesseract instance.</param>
        /// <param name="image">The image.</param>
        /// <param name="rectangle">The rectangle to perform OCR in.</param>
        /// <returns>The found text as a UTF8 string.</returns>
        public static Task<string> ReadAsync(this TesseractPool tesseract, Image image, Rectangle rectangle)
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
                return tesseract.ReadAsync(data, bmp.Width, bmp.Height, 4, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
            }
        }

        private static byte[] BitmapToBytes(Image<Rgba32> bmp)
        {
            byte[] bytes = new byte[bmp.Width * bmp.Height * 4];
            int index = 0;

            for (int y = 0; y < bmp.Height; y++)
            {
                byte[] row = MemoryMarshal.AsBytes(bmp.GetPixelRowSpan(y)).ToArray();
                Array.Copy(row, 0, bytes, index, row.Length);
                index += row.Length;
            }

            return bytes;
        }
    }
}
