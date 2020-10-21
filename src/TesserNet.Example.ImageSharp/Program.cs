using System;
using System.IO;
using System.Reflection;
using SixLabors.ImageSharp;

namespace TesserNet.Example.ImageSharp
{
    public static class Program
    {
        public static void Main()
        {
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("TesserNet.Example.ImageSharp.img.png");
            Image image = Image.Load(stream);
            Tesseract tesseract = new Tesseract();

            Console.WriteLine(tesseract.Read(image).Trim());

            stream.Dispose();
            image.Dispose();
            tesseract.Dispose();
        }
    }
}
