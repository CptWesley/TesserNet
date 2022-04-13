using System.IO;
using SkiaSharp;
using Xunit;
using static AssertNet.Assertions;

namespace TesserNet.Tests
{
    /// <summary>
    /// Contains simple tests.
    /// </summary>
    public static class Tests
    {
        private const string FileName1 = "img.png";
        private const string FileContent1 = "Hello world!";
        private static readonly ITesseract Tess = new Tesseract();

        /// <summary>
        /// Checks that the ImageSharp implementation works for simple image.
        /// </summary>
        [Fact]
        public static void ImageSharp()
        {
            using Stream s = ImageLoader.LoadStream(FileName1);
            using var img = SixLabors.ImageSharp.Image.Load(s);
            AssertThat(Tess.Read(img).Trim()).IsEqualTo(FileContent1);
        }

        /// <summary>
        /// Checks that the SkiaSharp implementation works for simple image.
        /// </summary>
        [Fact]
        public static void SkiaSharp()
        {
            using Stream s = ImageLoader.LoadStream(FileName1);
            using var img = SKBitmap.Decode(s);
            AssertThat(Tess.Read(img).Trim()).IsEqualTo(FileContent1);
        }

        /// <summary>
        /// Checks that the SkiaSharp implementation works for simple image.
        /// </summary>
        [Fact]
        public static void SystemDrawing()
        {
            using Stream s = ImageLoader.LoadStream(FileName1);
            using var img = System.Drawing.Image.FromStream(s);
            AssertThat(Tess.Read(img).Trim()).IsEqualTo(FileContent1);
        }
    }
}
