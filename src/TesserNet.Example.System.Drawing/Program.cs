using System;
using System.Drawing;
using System.IO;
using System.Reflection;

namespace TesserNet.Example.System.Drawing
{
    public static class Program
    {
        public static void Main()
        {
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("TesserNet.Example.System.Drawing.img.png");
            Image image = Image.FromStream(stream);
            Tesseract tesseract = new Tesseract();

            Console.WriteLine(tesseract.Read(image).Trim());

            stream.Dispose();
            image.Dispose();
            tesseract.Dispose();
        }
    }
}
