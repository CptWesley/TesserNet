using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TesserNet.Internal
{
    /// <summary>
    /// Provides implementation for a simple asynchronous queue.
    /// </summary>
    /// <typeparam name="T">Type of elements stored in the queue.</typeparam>
    internal class AsyncQueue<T> : IDisposable
    {
        private readonly Queue<T> queue = new Queue<T>();
        private readonly SemaphoreSlim mutation = new SemaphoreSlim(1, 1);
        private readonly SemaphoreSlim availability = new SemaphoreSlim(0);
        private bool isDisposed;

        /// <summary>
        /// Gets the count.
        /// </summary>
        public int Count => queue.Count;

        /// <summary>
        /// Enqueues a value.
        /// </summary>
        /// <param name="value">The value to enqueue.</param>
        /// <returns>A task that performs the enqueing.</returns>
        public async Task EnqueueAsync(T value)
        {
            await mutation.WaitAsync().ConfigureAwait(false);
            queue.Enqueue(value);
            mutation.Release();
            availability.Release();
        }

        /// <summary>
        /// Dequeues a value asynchronously.
        /// </summary>
        /// <returns>A task which awaits a value to dequeue.</returns>
        public async Task<T> DequeueAsync()
        {
            await availability.WaitAsync().ConfigureAwait(false);
            await mutation.WaitAsync().ConfigureAwait(false);
            T result = queue.Dequeue();
            mutation.Release();
            return result;
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
            if (isDisposed)
            {
                return;
            }

            isDisposed = true;

            if (disposing)
            {
                availability.Dispose();
                mutation.Dispose();
            }
        }
    }
}
