﻿using System;
using System.IO;
using System.Threading.Tasks;
using TesserNet.Internal;

namespace TesserNet
{
    /// <summary>
    /// Provides high level bindings for the Tesseract API.
    /// </summary>
    public class Tesseract : TesseractBase
    {
        private readonly TesseractApi api;
        private readonly IntPtr handle;
        private readonly object lck = new object();
        private bool isDisposed;
        private TesseractOptions? lastOptions;

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
        public Tesseract(Action<TesseractOptions> options)
            : this()
        {
            if (options != null)
            {
                options(Options);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tesseract"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public Tesseract(TesseractOptions options)
            : base(options)
        {
            api = TesseractApi.Create();
            handle = api.TessBaseAPICreate();
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="Tesseract"/> class.
        /// </summary>
        ~Tesseract()
            => Dispose(false);

        /// <inheritdoc/>
        public override unsafe string Read(IntPtr data, int width, int height, int bytesPerPixel, int rectX, int rectY, int rectWidth, int rectHeight)
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

                if (!Options.Equals(lastOptions!))
                {
                    lastOptions = Options.Copy();
                    Init();
                }

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

                string result;
                try
                {
                    result = api.TessBaseAPIGetUTF8Text(handle);
                }
                catch
                {
                    throw new TesseractException("Error while performing OCR.");
                }

                try
                {
                    api.TessBaseAPIClear(handle);
                }
                catch
                {
                    throw new TesseractException("Error while clearing result data.");
                }

                return result;
            }
        }

        /// <inheritdoc/>
        public override Task<string> ReadAsync(IntPtr data, int width, int height, int bytesPerPixel, int rectX, int rectY, int rectWidth, int rectHeight)
            => Task.Run(() => Read(data, width, height, bytesPerPixel, rectX, rectY, rectWidth, rectHeight));

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
                throw new TesseractException($"Error while initializing Tesseract with data file '{Path.Combine(Options.DataPath, $"{Options.Language}.traineddata")}'. It's possible the training data was not found or the data does not support the current OCR engine mode.");
            }

            try
            {
                api.TessBaseAPISetPageSegMode(handle, (int)Options.PageSegmentation);
            }
            catch
            {
                throw new TesseractException("Error while setting page segmentation mode.");
            }

            try
            {
                if (!api.TessBaseAPISetVariable(handle, "tessedit_char_whitelist", string.IsNullOrWhiteSpace(Options.Whitelist) ? string.Empty : Options.Whitelist))
                {
                    throw new TesseractException("Setting whitelist unsuccesful.");
                }
            }
            catch
            {
                throw new TesseractException("Error while setting whitelist.");
            }

            try
            {
                if (!api.TessBaseAPISetVariable(handle, "tessedit_char_blacklist", string.IsNullOrWhiteSpace(Options.Blacklist) ? string.Empty : Options.Blacklist))
                {
                    throw new TesseractException("Setting blacklist unsuccesful.");
                }
            }
            catch
            {
                throw new TesseractException("Error while setting blacklist.");
            }

            try
            {
                if (!api.TessBaseAPISetVariable(handle, "classify_bln_numeric_mode", Options.Numeric ? "1" : "0"))
                {
                    throw new TesseractException("Setting numeric mode unsuccesful.");
                }
            }
            catch
            {
                throw new TesseractException("Error while setting numeric mode.");
            }

            if (!string.IsNullOrWhiteSpace(Options.Config))
            {
                try
                {
                    api.TessBaseAPIReadConfigFile(handle, Options.Config);
                }
                catch
                {
                    throw new TesseractException($"Error while loading config: '{Options.Config}'.");
                }
            }
        }
    }
}
