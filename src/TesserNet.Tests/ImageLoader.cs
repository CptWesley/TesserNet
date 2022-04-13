using System.IO;
using System.Reflection;

namespace TesserNet.Tests
{
    /// <summary>
    /// Used to load images.
    /// </summary>
    internal static class ImageLoader
    {
        /// <summary>
        /// Loads an image as a stream.
        /// </summary>
        /// <param name="fileName">The filename.</param>
        /// <returns>The stream.</returns>
        public static Stream LoadStream(string fileName)
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            return asm.GetManifestResourceStream($"TesserNet.Tests.Resources.{fileName}");
        }

        /// <summary>
        /// Loads an image as a byte array.
        /// </summary>
        /// <param name="fileName">The filename.</param>
        /// <returns>The stream.</returns>
        public static byte[] LoadByteArray(string fileName)
        {
            using MemoryStream ms = new MemoryStream();
            using Stream s = LoadStream(fileName);
            s.CopyTo(ms);
            return ms.ToArray();
        }
    }
}
