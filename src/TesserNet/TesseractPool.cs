using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TesserNet.Internal;

namespace TesserNet
{
    /// <summary>
    /// Scheduler for easier management of multiple tesseract instances.
    /// </summary>
    public class TesseractPool : TesseractBase
    {
        private const int DefaultMaxPoolSize = 6;

        private readonly LazyQueue<Tesseract> waiting = new LazyQueue<Tesseract>();
        private readonly HashSet<Tesseract> tesseracts = new HashSet<Tesseract>();
        private readonly SemaphoreSlim semaphore = new SemaphoreSlim(1);
        private int busyCount;
        private int maxPoolSize;
        private bool isDisposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="TesseractPool"/> class.
        /// </summary>
        public TesseractPool()
            : this(DefaultMaxPoolSize)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TesseractPool"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="maxPoolSize">Maximum size of the pool.</param>
        public TesseractPool(Action<TesseractOptions> options, int maxPoolSize)
            : this(maxPoolSize)
        {
            if (options != null)
            {
                options(Options);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TesseractPool"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public TesseractPool(Action<TesseractOptions> options)
            : this(options, DefaultMaxPoolSize)
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
            : this(options, DefaultMaxPoolSize)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TesseractPool"/> class.
        /// </summary>
        /// <param name="options">The Tesseract options used for all spawned instances.</param>
        /// <param name="maxPoolSize">Maximum size of the pool.</param>
        public TesseractPool(TesseractOptions options, int maxPoolSize)
            : base(options)
            => (Options, this.maxPoolSize) = (options, maxPoolSize);

        /// <summary>
        /// Gets or sets the maximum size of the pool.
        /// </summary>
        public int MaxPoolSize
        {
            get => maxPoolSize;
            set => Resize(value);
        }

        /// <inheritdoc/>
        public override string Read(IntPtr data, int width, int height, int bytesPerPixel, int rectX, int rectY, int rectWidth, int rectHeight)
        {
            if (isDisposed)
            {
                throw new ObjectDisposedException(nameof(TesseractPool));
            }

            semaphore.Wait();

            Tesseract tesseract;
            try
            {
                if (waiting.Count > 0)
                {
                    tesseract = waiting.Dequeue();
                }
                else if (tesseracts.Count < MaxPoolSize)
                {
                    tesseract = new Tesseract();
                    tesseracts.Add(tesseract);
                }
                else
                {
                    tesseract = waiting.Dequeue();
                }

                tesseract.Options = Options.Copy();
            }
            finally
            {
                semaphore.Release();
            }

            string result = tesseract.Read(data, width, height, bytesPerPixel, rectX, rectY, rectWidth, rectHeight);
            waiting.Enqueue(tesseract);
            return result;
        }

        /// <inheritdoc/>
        public override async Task<string> ReadAsync(IntPtr data, int width, int height, int bytesPerPixel, int rectX, int rectY, int rectWidth, int rectHeight)
        {
            if (isDisposed)
            {
                throw new ObjectDisposedException(nameof(TesseractPool));
            }

            await semaphore.WaitAsync().ConfigureAwait(false);

            Tesseract tesseract;
            try
            {
                if (waiting.Count > 0)
                {
                    tesseract = await waiting.DequeueAsync().ConfigureAwait(false);
                }
                else if (tesseracts.Count < MaxPoolSize)
                {
                    tesseract = new Tesseract();
                    tesseracts.Add(tesseract);
                }
                else
                {
                    tesseract = await waiting.DequeueAsync().ConfigureAwait(false);
                }

                Interlocked.Increment(ref busyCount);
                tesseract.Options = Options.Copy();
            }
            finally
            {
                semaphore.Release();
            }

            Task<string> ocr = tesseract.ReadAsync(data, width, height, bytesPerPixel, rectX, rectY, rectWidth, rectHeight);
            _ = GoToWaiting(tesseract, ocr);
            return await ocr.ConfigureAwait(false);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (isDisposed)
            {
                return;
            }

            isDisposed = true;

            if (disposing)
            {
                semaphore.Wait();

                foreach (Tesseract tesseract in tesseracts)
                {
                    tesseract.Dispose();
                }

                waiting.Dispose();
                semaphore.Dispose();
            }
        }

        private async Task GoToWaiting(Tesseract t, Task<string> task)
        {
            await task.ConfigureAwait(false);
            Interlocked.Decrement(ref busyCount);
            await waiting.EnqueueAsync(t).ConfigureAwait(false);
        }

        private void Resize(int size)
        {
            maxPoolSize = size;

            if (!isDisposed)
            {
                _ = KillExcess();
            }
        }

        private async Task KillExcess()
        {
            await semaphore.WaitAsync().ConfigureAwait(false);

            while (busyCount + waiting.Count > maxPoolSize)
            {
                Tesseract tesseract = await waiting.DequeueAsync().ConfigureAwait(false);
                tesseracts.Remove(tesseract);
                tesseract.Dispose();
            }

            semaphore.Release();
        }
    }
}
