using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace TesserNet
{
    /// <summary>
    /// Scheduler for easier management of multiple tesseract instances.
    /// </summary>
    public class TesseractPool : IDisposable
    {
        private readonly BufferBlock<Tesseract> waiting = new BufferBlock<Tesseract>();
        private readonly HashSet<Tesseract> tesseracts = new HashSet<Tesseract>();
        private readonly SemaphoreSlim semaphore = new SemaphoreSlim(1);
        private int busyCount;
        private int maxPoolSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="TesseractPool"/> class.
        /// </summary>
        public TesseractPool()
            : this(6)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TesseractPool"/> class.
        /// </summary>
        /// <param name="maxPoolSize">Maximum size of the pool.</param>
        public TesseractPool(int maxPoolSize)
            : this(new TesseractOptions(), maxPoolSize)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TesseractPool"/> class.
        /// </summary>
        /// <param name="options">The Tesseract options used for all spawned instances.</param>
        public TesseractPool(TesseractOptions options)
            : this(options, 6)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TesseractPool"/> class.
        /// </summary>
        /// <param name="options">The Tesseract options used for all spawned instances.</param>
        /// <param name="maxPoolSize">Maximum size of the pool.</param>
        public TesseractPool(TesseractOptions options, int maxPoolSize)
            => (Options, this.maxPoolSize) = (options, maxPoolSize);

        /// <summary>
        /// Gets or sets the options.
        /// </summary>
        public TesseractOptions Options { get; set; }

        /// <summary>
        /// Gets or sets the maximum size of the pool.
        /// </summary>
        public int MaxPoolSize
        {
            get => maxPoolSize;
            set => Resize(value);
        }

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
        public async Task<string> ReadAsync(byte[] data, int width, int height, int bytesPerPixel, int rectX, int rectY, int rectWidth, int rectHeight)
        {
            await semaphore.WaitAsync().ConfigureAwait(false);
            Tesseract tesseract;
            if (waiting.Count > 0)
            {
                tesseract = waiting.Receive();
            }
            else if (tesseracts.Count < MaxPoolSize)
            {
                tesseract = new Tesseract();
                tesseracts.Add(tesseract);
            }
            else
            {
                tesseract = await waiting.ReceiveAsync().ConfigureAwait(false);
            }

            Interlocked.Increment(ref busyCount);
            tesseract.Options = Options.Copy();
            semaphore.Release();
            Task<string> ocr = tesseract.ReadAsync(data, width, height, bytesPerPixel, rectX, rectY, rectWidth, rectHeight);
            _ = GoToWaiting(tesseract, ocr);
            return await ocr.ConfigureAwait(false);
        }

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
            if (disposing)
            {
                semaphore.Wait();

                foreach (Tesseract tesseract in tesseracts)
                {
                    tesseract.Dispose();
                }

                semaphore.Release();
                semaphore.Dispose();
            }
        }

        private async Task GoToWaiting(Tesseract t, Task<string> task)
        {
            await task.ConfigureAwait(false);
            Interlocked.Decrement(ref busyCount);
            await waiting.SendAsync(t).ConfigureAwait(false);
        }

        private void Resize(int size)
        {
            maxPoolSize = size;
            _ = KillExcess();
        }

        private async Task KillExcess()
        {
            await semaphore.WaitAsync().ConfigureAwait(false);

            while (busyCount + waiting.Count > maxPoolSize)
            {
                Tesseract tesseract = await waiting.ReceiveAsync().ConfigureAwait(false);
                tesseracts.Remove(tesseract);
                tesseract.Dispose();
            }

            semaphore.Release();
        }
    }
}
