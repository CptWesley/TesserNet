using System;
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
        public static string Read(this ITesseract tesseract, Image image)
            => tesseract.Read(image, new Rectangle(-1, -1, -1, -1));

        /// <summary>
        /// Performs OCR on a rectangle inside the given image.
        /// </summary>
        /// <param name="tesseract">The tesseract instance.</param>
        /// <param name="image">The image.</param>
        /// <param name="rectangle">The rectangle to perform OCR in.</param>
        /// <returns>The found text as a UTF8 string.</returns>
        public static string Read(this ITesseract tesseract, Image image, Rectangle rectangle)
        {
            if (tesseract is null)
            {
                throw new ArgumentNullException(nameof(tesseract));
            }

            if (image is null)
            {
                throw new ArgumentNullException(nameof(image));
            }

            if (image is not Image<Rgba32> bmp)
            {
                bmp = image.CloneAs<Rgba32>();
            }

            IntPtr data = BitmapToBytes(bmp);
            string result = tesseract.Read(data, image.Width, image.Height, 4, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);

            if (bmp != image)
            {
                bmp.Dispose();
            }

            return result;
        }

        /// <summary>
        /// Performs OCR on the given image.
        /// </summary>
        /// <param name="tesseract">The tesseract instance.</param>
        /// <param name="image">The image.</param>
        /// <returns>The found text as a UTF8 string.</returns>
        public static Task<string> ReadAsync(this ITesseract tesseract, Image image)
            => tesseract.ReadAsync(image, new Rectangle(-1, -1, -1, -1));

        /// <summary>
        /// Performs OCR on a rectangle inside the given image.
        /// </summary>
        /// <param name="tesseract">The tesseract instance.</param>
        /// <param name="image">The image.</param>
        /// <param name="rectangle">The rectangle to perform OCR in.</param>
        /// <returns>The found text as a UTF8 string.</returns>
        public static Task<string> ReadAsync(this ITesseract tesseract, Image image, Rectangle rectangle)
        {
            if (tesseract is null)
            {
                throw new ArgumentNullException(nameof(tesseract));
            }

            if (image is null)
            {
                throw new ArgumentNullException(nameof(image));
            }

            if (image is not Image<Rgba32> bmp)
            {
                bmp = image.CloneAs<Rgba32>();
            }

            IntPtr data = BitmapToBytes(bmp);

            Task<string> resultTask = tesseract.ReadAsync(data, image.Width, image.Height, 4, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);

            return resultTask.ContinueWith(r =>
            {
                if (bmp != image)
                {
                    bmp.Dispose();
                }

                return r.Result;
            });
        }

        private static unsafe IntPtr BitmapToBytes(Image<Rgba32> image)
        {
            if (!image.DangerousTryGetSinglePixelMemory(out Memory<Rgba32> memory))
            {
                throw new TesseractException($"Could not get image pixels.");
            }

            fixed (Rgba32* ptr = memory.Span)
            {
                return new IntPtr(ptr);
            }
        }
    }
}
