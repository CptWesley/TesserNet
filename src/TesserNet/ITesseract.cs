using System;
using System.Threading.Tasks;

namespace TesserNet
{
    /// <summary>
    /// Interface for Tesseract instances.
    /// </summary>
    public unsafe interface ITesseract : IDisposable
    {
        /// <summary>
        /// Gets or sets the options.
        /// </summary>
        TesseractOptions Options { get; set; }

        /// <summary>
        /// Performs OCR on the given image.
        /// </summary>
        /// <param name="data">The bytes of the image.</param>
        /// <param name="width">The width of the image.</param>
        /// <param name="height">The height of the image.</param>
        /// <param name="bytesPerPixel">The number of bytes per pixel.</param>
        /// <returns>The found text as a UTF8 string.</returns>
        string Read(byte[] data, int width, int height, int bytesPerPixel);

        /// <summary>
        /// Performs OCR on the given image.
        /// </summary>
        /// <param name="data">The bytes of the image.</param>
        /// <param name="width">The width of the image.</param>
        /// <param name="height">The height of the image.</param>
        /// <param name="bytesPerPixel">The number of bytes per pixel.</param>
        /// <returns>The found text as a UTF8 string.</returns>
        Task<string> ReadAsync(byte[] data, int width, int height, int bytesPerPixel);

        /// <summary>
        /// Performs OCR on a rectangle inside the given image.
        /// </summary>
        /// <param name="data">The bytes of the image.</param>
        /// <param name="width">The width of the image.</param>
        /// <param name="height">The height of the image.</param>
        /// <param name="bytesPerPixel">The number of bytes per pixel.</param>
        /// <param name="rectX">The X coordinate of the rectangle.</param>
        /// <param name="rectY">The Y coordinate of the rectangle.</param>
        /// <param name="rectWidth">The width of the rectangle.</param>
        /// <param name="rectHeight">The height of the rectangle.</param>
        /// <returns>The found text as a UTF8 string.</returns>
        string Read(byte[] data, int width, int height, int bytesPerPixel, int rectX, int rectY, int rectWidth, int rectHeight);

        /// <summary>
        /// Performs OCR on a rectangle inside the given image.
        /// </summary>
        /// <param name="data">The bytes of the image.</param>
        /// <param name="width">The width of the image.</param>
        /// <param name="height">The height of the image.</param>
        /// <param name="bytesPerPixel">The number of bytes per pixel.</param>
        /// <param name="rectX">The X coordinate of the rectangle.</param>
        /// <param name="rectY">The Y coordinate of the rectangle.</param>
        /// <param name="rectWidth">The width of the rectangle.</param>
        /// <param name="rectHeight">The height of the rectangle.</param>
        /// <returns>The found text as a UTF8 string.</returns>
        Task<string> ReadAsync(byte[] data, int width, int height, int bytesPerPixel, int rectX, int rectY, int rectWidth, int rectHeight);

        /// <summary>
        /// Performs OCR on the given image.
        /// </summary>
        /// <param name="data">The bytes of the image.</param>
        /// <param name="width">The width of the image.</param>
        /// <param name="height">The height of the image.</param>
        /// <param name="bytesPerPixel">The number of bytes per pixel.</param>
        /// <returns>The found text as a UTF8 string.</returns>
        string Read(Memory<byte> data, int width, int height, int bytesPerPixel);

        /// <summary>
        /// Performs OCR on the given image.
        /// </summary>
        /// <param name="data">The bytes of the image.</param>
        /// <param name="width">The width of the image.</param>
        /// <param name="height">The height of the image.</param>
        /// <param name="bytesPerPixel">The number of bytes per pixel.</param>
        /// <returns>The found text as a UTF8 string.</returns>
        Task<string> ReadAsync(Memory<byte> data, int width, int height, int bytesPerPixel);

        /// <summary>
        /// Performs OCR on a rectangle inside the given image.
        /// </summary>
        /// <param name="data">The bytes of the image.</param>
        /// <param name="width">The width of the image.</param>
        /// <param name="height">The height of the image.</param>
        /// <param name="bytesPerPixel">The number of bytes per pixel.</param>
        /// <param name="rectX">The X coordinate of the rectangle.</param>
        /// <param name="rectY">The Y coordinate of the rectangle.</param>
        /// <param name="rectWidth">The width of the rectangle.</param>
        /// <param name="rectHeight">The height of the rectangle.</param>
        /// <returns>The found text as a UTF8 string.</returns>
        string Read(Memory<byte> data, int width, int height, int bytesPerPixel, int rectX, int rectY, int rectWidth, int rectHeight);

        /// <summary>
        /// Performs OCR on a rectangle inside the given image.
        /// </summary>
        /// <param name="data">The bytes of the image.</param>
        /// <param name="width">The width of the image.</param>
        /// <param name="height">The height of the image.</param>
        /// <param name="bytesPerPixel">The number of bytes per pixel.</param>
        /// <param name="rectX">The X coordinate of the rectangle.</param>
        /// <param name="rectY">The Y coordinate of the rectangle.</param>
        /// <param name="rectWidth">The width of the rectangle.</param>
        /// <param name="rectHeight">The height of the rectangle.</param>
        /// <returns>The found text as a UTF8 string.</returns>
        Task<string> ReadAsync(Memory<byte> data, int width, int height, int bytesPerPixel, int rectX, int rectY, int rectWidth, int rectHeight);

        /// <summary>
        /// Performs OCR on the given image.
        /// </summary>
        /// <param name="data">The bytes of the image.</param>
        /// <param name="width">The width of the image.</param>
        /// <param name="height">The height of the image.</param>
        /// <param name="bytesPerPixel">The number of bytes per pixel.</param>
        /// <returns>The found text as a UTF8 string.</returns>
        string Read(Span<byte> data, int width, int height, int bytesPerPixel);

        /// <summary>
        /// Performs OCR on the given image.
        /// </summary>
        /// <param name="data">The bytes of the image.</param>
        /// <param name="width">The width of the image.</param>
        /// <param name="height">The height of the image.</param>
        /// <param name="bytesPerPixel">The number of bytes per pixel.</param>
        /// <returns>The found text as a UTF8 string.</returns>
        Task<string> ReadAsync(Span<byte> data, int width, int height, int bytesPerPixel);

        /// <summary>
        /// Performs OCR on a rectangle inside the given image.
        /// </summary>
        /// <param name="data">The bytes of the image.</param>
        /// <param name="width">The width of the image.</param>
        /// <param name="height">The height of the image.</param>
        /// <param name="bytesPerPixel">The number of bytes per pixel.</param>
        /// <param name="rectX">The X coordinate of the rectangle.</param>
        /// <param name="rectY">The Y coordinate of the rectangle.</param>
        /// <param name="rectWidth">The width of the rectangle.</param>
        /// <param name="rectHeight">The height of the rectangle.</param>
        /// <returns>The found text as a UTF8 string.</returns>
        string Read(Span<byte> data, int width, int height, int bytesPerPixel, int rectX, int rectY, int rectWidth, int rectHeight);

        /// <summary>
        /// Performs OCR on a rectangle inside the given image.
        /// </summary>
        /// <param name="data">The bytes of the image.</param>
        /// <param name="width">The width of the image.</param>
        /// <param name="height">The height of the image.</param>
        /// <param name="bytesPerPixel">The number of bytes per pixel.</param>
        /// <param name="rectX">The X coordinate of the rectangle.</param>
        /// <param name="rectY">The Y coordinate of the rectangle.</param>
        /// <param name="rectWidth">The width of the rectangle.</param>
        /// <param name="rectHeight">The height of the rectangle.</param>
        /// <returns>The found text as a UTF8 string.</returns>
        Task<string> ReadAsync(Span<byte> data, int width, int height, int bytesPerPixel, int rectX, int rectY, int rectWidth, int rectHeight);

        /// <summary>
        /// Performs OCR on the given image.
        /// </summary>
        /// <param name="data">The bytes of the image.</param>
        /// <param name="width">The width of the image.</param>
        /// <param name="height">The height of the image.</param>
        /// <param name="bytesPerPixel">The number of bytes per pixel.</param>
        /// <returns>The found text as a UTF8 string.</returns>
        string Read(byte* data, int width, int height, int bytesPerPixel);

        /// <summary>
        /// Performs OCR on the given image.
        /// </summary>
        /// <param name="data">The bytes of the image.</param>
        /// <param name="width">The width of the image.</param>
        /// <param name="height">The height of the image.</param>
        /// <param name="bytesPerPixel">The number of bytes per pixel.</param>
        /// <returns>The found text as a UTF8 string.</returns>
        Task<string> ReadAsync(byte* data, int width, int height, int bytesPerPixel);

        /// <summary>
        /// Performs OCR on a rectangle inside the given image.
        /// </summary>
        /// <param name="data">The bytes of the image.</param>
        /// <param name="width">The width of the image.</param>
        /// <param name="height">The height of the image.</param>
        /// <param name="bytesPerPixel">The number of bytes per pixel.</param>
        /// <param name="rectX">The X coordinate of the rectangle.</param>
        /// <param name="rectY">The Y coordinate of the rectangle.</param>
        /// <param name="rectWidth">The width of the rectangle.</param>
        /// <param name="rectHeight">The height of the rectangle.</param>
        /// <returns>The found text as a UTF8 string.</returns>
        string Read(byte* data, int width, int height, int bytesPerPixel, int rectX, int rectY, int rectWidth, int rectHeight);

        /// <summary>
        /// Performs OCR on a rectangle inside the given image.
        /// </summary>
        /// <param name="data">The bytes of the image.</param>
        /// <param name="width">The width of the image.</param>
        /// <param name="height">The height of the image.</param>
        /// <param name="bytesPerPixel">The number of bytes per pixel.</param>
        /// <param name="rectX">The X coordinate of the rectangle.</param>
        /// <param name="rectY">The Y coordinate of the rectangle.</param>
        /// <param name="rectWidth">The width of the rectangle.</param>
        /// <param name="rectHeight">The height of the rectangle.</param>
        /// <returns>The found text as a UTF8 string.</returns>
        Task<string> ReadAsync(byte* data, int width, int height, int bytesPerPixel, int rectX, int rectY, int rectWidth, int rectHeight);

        /// <summary>
        /// Performs OCR on the given image.
        /// </summary>
        /// <param name="data">The bytes of the image.</param>
        /// <param name="width">The width of the image.</param>
        /// <param name="height">The height of the image.</param>
        /// <param name="bytesPerPixel">The number of bytes per pixel.</param>
        /// <returns>The found text as a UTF8 string.</returns>
        string Read(IntPtr data, int width, int height, int bytesPerPixel);

        /// <summary>
        /// Performs OCR on the given image.
        /// </summary>
        /// <param name="data">The bytes of the image.</param>
        /// <param name="width">The width of the image.</param>
        /// <param name="height">The height of the image.</param>
        /// <param name="bytesPerPixel">The number of bytes per pixel.</param>
        /// <returns>The found text as a UTF8 string.</returns>
        Task<string> ReadAsync(IntPtr data, int width, int height, int bytesPerPixel);

        /// <summary>
        /// Performs OCR on a rectangle inside the given image.
        /// </summary>
        /// <param name="data">The bytes of the image.</param>
        /// <param name="width">The width of the image.</param>
        /// <param name="height">The height of the image.</param>
        /// <param name="bytesPerPixel">The number of bytes per pixel.</param>
        /// <param name="rectX">The X coordinate of the rectangle.</param>
        /// <param name="rectY">The Y coordinate of the rectangle.</param>
        /// <param name="rectWidth">The width of the rectangle.</param>
        /// <param name="rectHeight">The height of the rectangle.</param>
        /// <returns>The found text as a UTF8 string.</returns>
        string Read(IntPtr data, int width, int height, int bytesPerPixel, int rectX, int rectY, int rectWidth, int rectHeight);

        /// <summary>
        /// Performs OCR on a rectangle inside the given image.
        /// </summary>
        /// <param name="data">The bytes of the image.</param>
        /// <param name="width">The width of the image.</param>
        /// <param name="height">The height of the image.</param>
        /// <param name="bytesPerPixel">The number of bytes per pixel.</param>
        /// <param name="rectX">The X coordinate of the rectangle.</param>
        /// <param name="rectY">The Y coordinate of the rectangle.</param>
        /// <param name="rectWidth">The width of the rectangle.</param>
        /// <param name="rectHeight">The height of the rectangle.</param>
        /// <returns>The found text as a UTF8 string.</returns>
        Task<string> ReadAsync(IntPtr data, int width, int height, int bytesPerPixel, int rectX, int rectY, int rectWidth, int rectHeight);
    }
}
