using System;
using TesserNet.Internal;

namespace TesserNet
{
    /// <summary>
    /// Represents the options used for invoking Tesseract.
    /// </summary>
    public class TesseractOptions : IEquatable<TesseractOptions>
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

        /// <summary>
        /// Gets or sets the page segmentation option.
        /// </summary>
        public PageSegmentation PageSegmentation { get; set; } = PageSegmentation.Block;

        /// <summary>
        /// Gets or sets the whitelist.
        /// </summary>
        public string Whitelist { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the blacklist.
        /// </summary>
        public string Blacklist { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the thing we try to parse is numeric.
        /// </summary>
        public bool Numeric { get; set; }

        /// <summary>
        /// Gets or sets the configuration name or path.
        /// </summary>
        public string Config { get; set; } = string.Empty;

        /// <summary>
        /// Creates a copy of the options.
        /// </summary>
        /// <returns>A copy of the options.</returns>
        public TesseractOptions Copy()
            => new TesseractOptions
            {
                Language = this.Language,
                DataPath = this.DataPath,
                EngineMode = this.EngineMode,
                PixelsPerInch = this.PixelsPerInch,
                PageSegmentation = this.PageSegmentation,
                Whitelist = this.Whitelist,
                Blacklist = this.Blacklist,
                Numeric = this.Numeric,
                Config = this.Config,
            };

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj is TesseractOptions other)
            {
                return Equals(other);
            }

            return false;
        }

        /// <inheritdoc/>
        public bool Equals(TesseractOptions other)
        {
            if (other is null)
            {
                return false;
            }

            return Language == other.Language
                && DataPath == other.DataPath
                && EngineMode == other.EngineMode
                && PixelsPerInch == other.PixelsPerInch
                && PageSegmentation == other.PageSegmentation
                && Whitelist == other.Whitelist
                && Blacklist == other.Blacklist
                && Numeric == other.Numeric
                && Config == other.Config;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
            => Language.GetHashCode()
            + (2 * DataPath.GetHashCode())
            + (3 * (int)(EngineMode + 1))
            + (4 * (PixelsPerInch + 1))
            + (5 * (int)(PageSegmentation + 1))
            + (6 * Whitelist.GetHashCode())
            + (7 * Blacklist.GetHashCode())
            + (Numeric ? 8 : 0)
            + (9 * Config.GetHashCode());
    }
}
