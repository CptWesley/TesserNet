using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
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

            using (Bitmap bmp = new Bitmap(image))
            {
                byte[] data = BitmapToBytes(bmp);
                int bpp = Image.GetPixelFormatSize(bmp.PixelFormat) / 8;
                return tesseract.Read(data, bmp.Width, bmp.Height, bpp, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
            }
        }

        /// <summary>
        /// Performs OCR on the given image.
        /// </summary>
        /// <param name="tesseract">The tesseract instance.</param>
        /// <param name="image">The image.</param>
        /// <returns>The found text as a UTF8 string.</returns>
        public static Task<string> ReadAsync(this Tesseract tesseract, Image image)
            => tesseract.ReadAsync(image, new Rectangle(-1, -1, -1, -1));

        /// <summary>
        /// Performs OCR on a rectangle inside the given image.
        /// </summary>
        /// <param name="tesseract">The tesseract instance.</param>
        /// <param name="image">The image.</param>
        /// <param name="rectangle">The rectangle to perform OCR in.</param>
        /// <returns>The found text as a UTF8 string.</returns>
        public static Task<string> ReadAsync(this Tesseract tesseract, Image image, Rectangle rectangle)
        {
            if (tesseract is null)
            {
                throw new ArgumentNullException(nameof(tesseract));
            }

            using (Bitmap bmp = new Bitmap(image))
            {
                byte[] data = BitmapToBytes(bmp);
                int bpp = Image.GetPixelFormatSize(bmp.PixelFormat) / 8;
                return tesseract.ReadAsync(data, bmp.Width, bmp.Height, bpp, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
            }
        }

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

            using (Bitmap bmp = new Bitmap(image))
            {
                byte[] data = BitmapToBytes(bmp);
                int bpp = Image.GetPixelFormatSize(bmp.PixelFormat) / 8;
                return tesseract.ReadAsync(data, bmp.Width, bmp.Height, bpp, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
            }
        }

        private static byte[] BitmapToBytes(Bitmap bmp)
        {
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, bmp.PixelFormat);
            IntPtr ptr = bmpData.Scan0;
            int size = bmp.Width * bmp.Height * Image.GetPixelFormatSize(bmp.PixelFormat) / 8;
            byte[] bytes = new byte[size];
            Marshal.Copy(ptr, bytes, 0, size);
            bmp.UnlockBits(bmpData);
            return bytes;
        }
    }
}
