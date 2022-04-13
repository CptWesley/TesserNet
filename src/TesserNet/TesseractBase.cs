using System;
using System.Threading.Tasks;

namespace TesserNet
{
    /// <summary>
    /// Abstract base class for Tesseract instances.
    /// </summary>
    public unsafe abstract class TesseractBase : ITesseract
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TesseractBase"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public TesseractBase(TesseractOptions options)
            => Options = options;

        /// <inheritdoc/>
        public TesseractOptions Options { get; set; }

        /// <inheritdoc/>
        public abstract string Read(IntPtr data, int width, int height, int bytesPerPixel, int rectX, int rectY, int rectWidth, int rectHeight);

        /// <inheritdoc/>
        public abstract Task<string> ReadAsync(IntPtr data, int width, int height, int bytesPerPixel, int rectX, int rectY, int rectWidth, int rectHeight);

        /// <inheritdoc/>
        public string Read(Span<byte> data, int width, int height, int bytesPerPixel, int rectX, int rectY, int rectWidth, int rectHeight)
        {
            fixed (byte* ptr = data)
            {
                return Read(ptr, width, height, bytesPerPixel, rectX, rectY, rectWidth, rectHeight);
            }
        }

        /// <inheritdoc/>
        public Task<string> ReadAsync(Span<byte> data, int width, int height, int bytesPerPixel, int rectX, int rectY, int rectWidth, int rectHeight)
        {
            fixed (byte* ptr = data)
            {
                return ReadAsync(ptr, width, height, bytesPerPixel, rectX, rectY, rectWidth, rectHeight);
            }
        }

        /// <inheritdoc/>
        public string Read(byte[] data, int width, int height, int bytesPerPixel)
            => Read(data, width, height, bytesPerPixel, -1, -1, -1, -1);

        /// <inheritdoc/>
        public string Read(byte[] data, int width, int height, int bytesPerPixel, int rectX, int rectY, int rectWidth, int rectHeight)
            => Read((Span<byte>)data, width, height, bytesPerPixel, rectX, rectY, rectWidth, rectHeight);

        /// <inheritdoc/>
        public string Read(Memory<byte> data, int width, int height, int bytesPerPixel)
            => Read(data, width, height, bytesPerPixel, -1, -1, -1, -1);

        /// <inheritdoc/>
        public string Read(Memory<byte> data, int width, int height, int bytesPerPixel, int rectX, int rectY, int rectWidth, int rectHeight)
            => Read(data.Span, width, height, bytesPerPixel, rectX, rectY, rectWidth, rectHeight);

        /// <inheritdoc/>
        public string Read(Span<byte> data, int width, int height, int bytesPerPixel)
            => Read(data, width, height, bytesPerPixel, -1, -1, -1, -1);

        /// <inheritdoc/>
        public string Read(byte* data, int width, int height, int bytesPerPixel)
            => Read(data, width, height, bytesPerPixel, -1, -1, -1, -1);

        /// <inheritdoc/>
        public string Read(byte* data, int width, int height, int bytesPerPixel, int rectX, int rectY, int rectWidth, int rectHeight)
            => Read(new IntPtr(data), width, height, bytesPerPixel, rectX, rectY, rectWidth, rectHeight);

        /// <inheritdoc/>
        public string Read(IntPtr data, int width, int height, int bytesPerPixel)
            => Read(data, width, height, bytesPerPixel, -1, -1, -1, -1);

        /// <inheritdoc/>
        public Task<string> ReadAsync(byte[] data, int width, int height, int bytesPerPixel)
            => ReadAsync(data, width, height, bytesPerPixel, -1, -1, -1, -1);

        /// <inheritdoc/>
        public Task<string> ReadAsync(byte[] data, int width, int height, int bytesPerPixel, int rectX, int rectY, int rectWidth, int rectHeight)
            => ReadAsync((Memory<byte>)data, width, height, bytesPerPixel, rectX, rectY, rectWidth, rectHeight);

        /// <inheritdoc/>
        public Task<string> ReadAsync(Memory<byte> data, int width, int height, int bytesPerPixel)
            => ReadAsync(data, width, height, bytesPerPixel, -1, -1, -1, -1);

        /// <inheritdoc/>
        public Task<string> ReadAsync(Memory<byte> data, int width, int height, int bytesPerPixel, int rectX, int rectY, int rectWidth, int rectHeight)
            => ReadAsync(data.Span, width, height, bytesPerPixel, rectX, rectY, rectWidth, rectHeight);

        /// <inheritdoc/>
        public Task<string> ReadAsync(Span<byte> data, int width, int height, int bytesPerPixel)
            => ReadAsync(data, width, height, bytesPerPixel, -1, -1, -1, -1);

        /// <inheritdoc/>
        public Task<string> ReadAsync(byte* data, int width, int height, int bytesPerPixel)
            => ReadAsync(data, width, height, bytesPerPixel, -1, -1, -1, -1);

        /// <inheritdoc/>
        public Task<string> ReadAsync(byte* data, int width, int height, int bytesPerPixel, int rectX, int rectY, int rectWidth, int rectHeight)
            => ReadAsync(new IntPtr(data), width, height, bytesPerPixel, rectX, rectY, rectWidth, rectHeight);

        /// <inheritdoc/>
        public Task<string> ReadAsync(IntPtr data, int width, int height, int bytesPerPixel)
            => ReadAsync(data, width, height, bytesPerPixel, -1, -1, -1, -1);

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected abstract void Dispose(bool disposing);
    }
}
