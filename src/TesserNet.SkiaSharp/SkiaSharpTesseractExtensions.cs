using System;
using System.Threading.Tasks;
using SkiaSharp;

namespace TesserNet
{
    /// <summary>
    /// Provides extension methods for the <see cref="Tesseract"/> class.
    /// </summary>
    public static class SkiaSharpTesseractExtensions
    {
        /// <summary>
        /// Performs OCR on the given image.
        /// </summary>
        /// <param name="tesseract">The tesseract instance.</param>
        /// <param name="image">The image.</param>
        /// <returns>The found text as a UTF8 string.</returns>
        public static string Read(this ITesseract tesseract, SKBitmap image)
            => tesseract.Read(image, new SKRect(-1, -1, -1, -1));

        /// <summary>
        /// Performs OCR on a rectangle inside the given image.
        /// </summary>
        /// <param name="tesseract">The tesseract instance.</param>
        /// <param name="image">The image.</param>
        /// <param name="rectangle">The rectangle to perform OCR in.</param>
        /// <returns>The found text as a UTF8 string.</returns>
        public static string Read(this ITesseract tesseract, SKBitmap image, SKRect rectangle)
        {
            if (tesseract is null)
            {
                throw new ArgumentNullException(nameof(tesseract));
            }

            if (image is null)
            {
                throw new ArgumentNullException(nameof(image));
            }

            IntPtr data = BitmapToBytes(image);
            return tesseract.Read(data, image.Width, image.Height, 4, (int)rectangle.Left, (int)rectangle.Top, (int)rectangle.Width, (int)rectangle.Height);
        }

        /// <summary>
        /// Performs OCR on the given image.
        /// </summary>
        /// <param name="tesseract">The tesseract instance.</param>
        /// <param name="image">The image.</param>
        /// <returns>The found text as a UTF8 string.</returns>
        public static Task<string> ReadAsync(this ITesseract tesseract, SKBitmap image)
            => tesseract.ReadAsync(image, new SKRect(-1, -1, -1, -1));

        /// <summary>
        /// Performs OCR on a rectangle inside the given image.
        /// </summary>
        /// <param name="tesseract">The tesseract instance.</param>
        /// <param name="image">The image.</param>
        /// <param name="rectangle">The rectangle to perform OCR in.</param>
        /// <returns>The found text as a UTF8 string.</returns>
        public static Task<string> ReadAsync(this ITesseract tesseract, SKBitmap image, SKRect rectangle)
        {
            if (tesseract is null)
            {
                throw new ArgumentNullException(nameof(tesseract));
            }

            if (image is null)
            {
                throw new ArgumentNullException(nameof(image));
            }

            IntPtr data = BitmapToBytes(image);
            return tesseract.ReadAsync(data, image.Width, image.Height, 4, (int)rectangle.Left, (int)rectangle.Top, (int)rectangle.Width, (int)rectangle.Height);
        }

        /// <summary>
        /// Performs OCR on the given image.
        /// </summary>
        /// <param name="tesseract">The tesseract instance.</param>
        /// <param name="image">The image.</param>
        /// <returns>The found text as a UTF8 string.</returns>
        public static string Read(this ITesseract tesseract, SKImage image)
            => tesseract.Read(SKBitmap.FromImage(image));

        /// <summary>
        /// Performs OCR on a rectangle inside the given image.
        /// </summary>
        /// <param name="tesseract">The tesseract instance.</param>
        /// <param name="image">The image.</param>
        /// <param name="rectangle">The rectangle to perform OCR in.</param>
        /// <returns>The found text as a UTF8 string.</returns>
        public static string Read(this ITesseract tesseract, SKImage image, SKRect rectangle)
            => tesseract.Read(SKBitmap.FromImage(image), rectangle);

        /// <summary>
        /// Performs OCR on the given image.
        /// </summary>
        /// <param name="tesseract">The tesseract instance.</param>
        /// <param name="image">The image.</param>
        /// <returns>The found text as a UTF8 string.</returns>
        public static Task<string> ReadAsync(this ITesseract tesseract, SKImage image)
            => tesseract.ReadAsync(SKBitmap.FromImage(image));

        /// <summary>
        /// Performs OCR on a rectangle inside the given image.
        /// </summary>
        /// <param name="tesseract">The tesseract instance.</param>
        /// <param name="image">The image.</param>
        /// <param name="rectangle">The rectangle to perform OCR in.</param>
        /// <returns>The found text as a UTF8 string.</returns>
        public static Task<string> ReadAsync(this ITesseract tesseract, SKImage image, SKRect rectangle)
            => tesseract.ReadAsync(SKBitmap.FromImage(image), rectangle);

        private static unsafe IntPtr BitmapToBytes(SKBitmap bmp)
        {
            fixed (byte* ptr = bmp.GetPixelSpan())
            {
                return new IntPtr(ptr);
            }
        }
    }
}
