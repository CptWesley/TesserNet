using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;

namespace TesserNet
{
    /// <summary>
    /// Provides extension methods for the <see cref="Tesseract"/> class.
    /// </summary>
    public static class SystemDrawingTesseractExtensions
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
        [SuppressMessage("Reliability", "CA2000", Justification = "Bitmap is disposed if new one was created.")]
        public static string Read(this ITesseract tesseract, Image image, Rectangle rectangle)
        {
            if (tesseract is null)
            {
                throw new ArgumentNullException(nameof(tesseract));
            }

            if (image is not Bitmap bmp)
            {
                bmp = new Bitmap(image);
            }

            IntPtr data = BitmapToBytes(bmp);
            int bpp = Image.GetPixelFormatSize(bmp.PixelFormat) / 8;
            string result = tesseract.Read(data, image.Width, image.Height, bpp, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);

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
        [SuppressMessage("Reliability", "CA2000", Justification = "Bitmap is disposed if new one was created.")]
        public static Task<string> ReadAsync(this ITesseract tesseract, Image image, Rectangle rectangle)
        {
            if (tesseract is null)
            {
                throw new ArgumentNullException(nameof(tesseract));
            }

            if (image is not Bitmap bmp)
            {
                bmp = new Bitmap(image);
            }

            IntPtr data = BitmapToBytes(bmp);
            int bpp = Image.GetPixelFormatSize(image.PixelFormat) / 8;
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

        private static IntPtr BitmapToBytes(Bitmap image)
        {
            BitmapData bmpData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, image.PixelFormat);
            IntPtr ptr = bmpData.Scan0;
            image.UnlockBits(bmpData);
            return ptr;
        }
    }
}
