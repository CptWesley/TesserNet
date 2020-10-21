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
    internal class LazyQueue<T> : IDisposable
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
        /// Enqueues a value asynchronously.
        /// </summary>
        /// <param name="value">The value to enqueue.</param>
        /// <returns>A task that performs the enqueing.</returns>
        public async Task EnqueueAsync(T value)
        {
            await mutation.WaitAsync().ConfigureAwait(false);
            EnqueueInternal(value);
        }

        /// <summary>
        /// Enqueues a value synchronously.
        /// </summary>
        /// <param name="value">The value to enqueue.</param>
        public void Enqueue(T value)
        {
            mutation.Wait();
            EnqueueInternal(value);
        }

        /// <summary>
        /// Dequeues a value asynchronously.
        /// </summary>
        /// <returns>A task which awaits a value to dequeue.</returns>
        public async Task<T> DequeueAsync()
        {
            await availability.WaitAsync().ConfigureAwait(false);
            await mutation.WaitAsync().ConfigureAwait(false);
            return DequeueInternal();
        }

        /// <summary>
        /// Dequeues a value synchronously.
        /// </summary>
        /// <returns>The value to dequeue.</returns>
        public T Dequeue()
        {
            availability.Wait();
            mutation.Wait();
            return DequeueInternal();
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

        private void EnqueueInternal(T value)
        {
            queue.Enqueue(value);
            mutation.Release();
            availability.Release();
        }

        private T DequeueInternal()
        {
            T result = queue.Dequeue();
            mutation.Release();
            return result;
        }
    }
}
