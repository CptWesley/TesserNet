using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using TesserNet.System.Drawing;

namespace TesserNet.Example.System.Drawing
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("TesserNet.Example.System.Drawing.img.png");
            Image image = Image.FromStream(stream);
            Tesseract tesseract = new Tesseract();

            Console.WriteLine(tesseract.Read(image));

            stream.Dispose();
            image.Dispose();
            tesseract.Dispose();
        }
    }
}
