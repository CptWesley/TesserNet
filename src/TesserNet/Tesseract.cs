using System;
using System.IO;
using System.Threading.Tasks;
using TesserNet.Internal;

namespace TesserNet
{
    /// <summary>
    /// Provides high level bindings for the Tesseract API.
    /// </summary>
    /// <seealso cref="IDisposable" />
    public class Tesseract : IDisposable
    {
        private readonly TesseractApi api;
        private readonly IntPtr handle;
        private readonly object lck = new object();
        private bool isDisposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="Tesseract"/> class.
        /// </summary>
        public Tesseract()
            : this(new TesseractOptions())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tesseract"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public Tesseract(TesseractOptions options)
        {
            Options = options;
            api = TesseractApi.Create();
            handle = api.TessBaseAPICreate();
        }

        /// <summary>
        /// Gets or sets the options.
        /// </summary>
        public TesseractOptions Options { get; set; }

        /// <summary>
        /// Performs OCR on the given image.
        /// </summary>
        /// <param name="data">The bytes of the image.</param>
        /// <param name="width">The width of the image.</param>
        /// <param name="height">The height of the image.</param>
        /// <param name="bytesPerPixel">The number of bytes per pixel.</param>
        /// <returns>The found text as a UTF8 string.</returns>
        public string Read(byte[] data, int width, int height, int bytesPerPixel)
            => Read(data, width, height, bytesPerPixel, -1, -1, -1, -1);

        /// <summary>
        /// Performs OCR on the given image.
        /// </summary>
        /// <param name="data">The bytes of the image.</param>
        /// <param name="width">The width of the image.</param>
        /// <param name="height">The height of the image.</param>
        /// <param name="bytesPerPixel">The number of bytes per pixel.</param>
        /// <returns>The found text as a UTF8 string.</returns>
        public Task<string> ReadAsync(byte[] data, int width, int height, int bytesPerPixel)
            => ReadAsync(data, width, height, bytesPerPixel, -1, -1, -1, -1);

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
        public string Read(byte[] data, int width, int height, int bytesPerPixel, int rectX, int rectY, int rectWidth, int rectHeight)
        {
            if (isDisposed)
            {
                throw new ObjectDisposedException(nameof(Tesseract));
            }

            lock (lck)
            {
                if (isDisposed)
                {
                    throw new ObjectDisposedException(nameof(Tesseract));
                }

                Init();

                try
                {
                    api.TessBaseAPISetImage(handle, data, width, height, bytesPerPixel, width * bytesPerPixel);
                }
                catch
                {
                    throw new TesseractException("Error while setting subject image.");
                }

                try
                {
                    api.TessBaseAPISetSourceResolution(handle, Options.PixelsPerInch);
                }
                catch
                {
                    throw new TesseractException("Error while setting resolution.");
                }

                if (rectX >= 0 && rectY >= 0 && rectWidth > 0 && rectHeight > 0)
                {
                    try
                    {
                        api.TessBaseAPISetRectangle(handle, rectX, rectY, rectWidth, rectHeight);
                    }
                    catch
                    {
                        throw new TesseractException("Error while setting a rectangle.");
                    }
                }

                try
                {
                    return api.TessBaseAPIGetUTF8Text(handle);
                }
                catch
                {
                    throw new TesseractException("Error while performing OCR.");
                }
            }
        }

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
        public async Task<string> ReadAsync(byte[] data, int width, int height, int bytesPerPixel, int rectX, int rectY, int rectWidth, int rectHeight)
            => await Task.Run(() => Read(data, width, height, bytesPerPixel, rectX, rectY, rectWidth, rectHeight)).ConfigureAwait(false);

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
        protected virtual void Dispose(bool disposing)
        {
            if (isDisposed)
            {
                return;
            }

            lock (lck)
            {
                api.TessBaseAPIDelete(handle);
            }

            isDisposed = true;
        }

        private void Init()
        {
            int result = api.TessBaseAPIInit1(handle, Options.DataPath, Options.Language, (int)Options.EngineMode, IntPtr.Zero, 0);
            if (result != 0)
            {
                throw new TesseractException($"Error while initializing Tesseract with data file '{Path.Combine(Options.DataPath, $"{Options.Language}.traineddata")}'.");
            }
        }
    }
}
