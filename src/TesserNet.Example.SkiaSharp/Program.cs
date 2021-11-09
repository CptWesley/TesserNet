using SkiaSharp;
using System;
using System.IO;
using System.Reflection;

namespace TesserNet.Example.ImageSharp
{
    public static class Program
    {
        public static void Main()
        {
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("TesserNet.Example.SkiaSharp.img.png");
            SKBitmap image = SKBitmap.Decode(stream);
            Tesseract tesseract = new Tesseract();

            Console.WriteLine(tesseract.Read(image).Trim());

            stream.Dispose();
            image.Dispose();
            tesseract.Dispose();
        }
    }
}
