using System;
using System.Runtime.InteropServices;

namespace TesserNet.Internal
{
    /// <summary>
    /// Windows implementation of the Tesseract API.
    /// </summary>
    /// <seealso cref="TesseractApi" />
    internal class WindowsTesseractApi : TesseractApi
    {
        /// <inheritdoc/>
        public override IntPtr TessBaseAPICreate()
            => NativeMethods.TessBaseAPICreate();

        /// <inheritdoc/>
        public override void TessBaseAPIDelete(IntPtr handle)
            => NativeMethods.TessBaseAPIDelete(handle);

        /// <inheritdoc/>
        public override string TessBaseAPIGetUTF8Text(IntPtr handle)
            => NativeMethods.TessBaseAPIGetUTF8Text(handle);

        /// <inheritdoc/>
        public override int TessBaseAPIInit1(IntPtr handle, string dataPath, string language, int oem, IntPtr configs, int configSize)
            => NativeMethods.TessBaseAPIInit1(handle, dataPath, language, oem, configs, configSize);

        /// <inheritdoc/>
        public override void TessBaseAPISetImage(IntPtr handle, byte[] data, int width, int height, int bytesPerPixel, int bytesPerLine)
            => NativeMethods.TessBaseAPISetImage(handle, data, width, height, bytesPerPixel, bytesPerLine);

        /// <inheritdoc/>
        public override void TessBaseAPISetSourceResolution(IntPtr handle, int ppi)
            => NativeMethods.TessBaseAPISetSourceResolution(handle, ppi);

        /// <inheritdoc/>
        public override void TessBaseAPISetRectangle(IntPtr handle, int x, int y, int width, int height)
            => NativeMethods.TessBaseAPISetRectangle(handle, x, y, width, height);

        private static class NativeMethods
        {
            private const string DllPath = "libtesseract500";

            [DllImport(DllPath, CharSet = CharSet.Auto, SetLastError = true)]
            public static extern IntPtr TessBaseAPICreate();

            [DllImport(DllPath, CharSet = CharSet.Auto, SetLastError = true)]
            public static extern void TessBaseAPIDelete(IntPtr handle);

            [DllImport(DllPath, CharSet = CharSet.Auto, SetLastError = true)]
            public static extern int TessBaseAPIInit1(IntPtr handle, [MarshalAs(UnmanagedType.LPStr)] string dataPath, [MarshalAs(UnmanagedType.LPStr)] string language, int oem, IntPtr configs, int configSize);

            [DllImport(DllPath, CharSet = CharSet.Auto, SetLastError = true)]
            public static extern void TessBaseAPISetImage(IntPtr handle, byte[] data, int width, int height, int bytesPerPixel, int bytesPerLine);

            [DllImport(DllPath, CharSet = CharSet.Auto, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.LPStr)]
            public static extern string TessBaseAPIGetUTF8Text(IntPtr handle);

            [DllImport(DllPath, CharSet = CharSet.Auto, SetLastError = true)]
            public static extern void TessBaseAPISetSourceResolution(IntPtr handle, int ppi);

            [DllImport(DllPath, CharSet = CharSet.Auto, SetLastError = true)]
            public static extern void TessBaseAPISetRectangle(IntPtr handle, int x, int y, int width, int height);
        }
    }
}
