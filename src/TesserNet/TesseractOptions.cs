using System;
using TesserNet.Internal;

namespace TesserNet
{
    /// <summary>
    /// Represents the options used for invoking Tesseract.
    /// </summary>
    public class TesseractOptions
    {
        /// <summary>
        /// Gets or sets the language.
        /// </summary>
        public string Language { get; set; } = "eng";

        /// <summary>
        /// Gets or sets the data path.
        /// </summary>
        public string DataPath { get; set; } = Environment.GetEnvironmentVariable("TESSDATA_PREFIX") ?? Loader.GetUnpackDirectory();

        /// <summary>
        /// Gets or sets the engine mode.
        /// </summary>
        public OcrEngineMode EngineMode { get; set; } = OcrEngineMode.Default;

        /// <summary>
        /// Gets or sets the pixels per inch.
        /// </summary>
        public int PixelsPerInch { get; set; } = 70;
    }
}
