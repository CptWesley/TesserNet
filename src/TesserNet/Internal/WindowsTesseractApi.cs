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
            => NativeMethods.TessBaseAPIGetUTF8Text(handle).ToUtf8String();

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

        /// <inheritdoc/>
        public override void TessBaseAPIClear(IntPtr handle)
            => NativeMethods.TessBaseAPIClear(handle);

        /// <inheritdoc/>
        public override void TessBaseAPISetPageSegMode(IntPtr handle, int mode)
            => NativeMethods.TessBaseAPISetPageSegMode(handle, mode);

        /// <inheritdoc/>
        public override bool TessBaseAPISetVariable(IntPtr handle, string key, string value)
            => NativeMethods.TessBaseAPISetVariable(handle, key, value);

        /// <inheritdoc/>
        public override void TessBaseAPIReadConfigFile(IntPtr handle, string file)
            => NativeMethods.TessBaseAPIReadConfigFile(handle, file);

        private static class NativeMethods
        {
            private const string DllPath = "libtesseract500";

            [DllImport(DllPath, CharSet = CharSet.Auto, SetLastError = true)]
            public static extern IntPtr TessBaseAPICreate();

            [DllImport(DllPath, CharSet = CharSet.Auto, SetLastError = true)]
            public static extern void TessBaseAPIDelete(IntPtr handle);

            [DllImport(DllPath, CharSet = CharSet.Auto, SetLastError = true)]
            public static extern void TessBaseAPIClear(IntPtr handle);

            [DllImport(DllPath, CharSet = CharSet.Auto, SetLastError = true)]
            public static extern int TessBaseAPIInit1(IntPtr handle, [MarshalAs(UnmanagedType.LPStr)] string dataPath, [MarshalAs(UnmanagedType.LPStr)] string language, int oem, IntPtr configs, int configSize);

            [DllImport(DllPath, CharSet = CharSet.Auto, SetLastError = true)]
            public static extern void TessBaseAPISetImage(IntPtr handle, byte[] data, int width, int height, int bytesPerPixel, int bytesPerLine);

            [DllImport(DllPath, CharSet = CharSet.Auto, SetLastError = true)]
            public static extern IntPtr TessBaseAPIGetUTF8Text(IntPtr handle);

            [DllImport(DllPath, CharSet = CharSet.Auto, SetLastError = true)]
            public static extern void TessBaseAPISetSourceResolution(IntPtr handle, int ppi);

            [DllImport(DllPath, CharSet = CharSet.Auto, SetLastError = true)]
            public static extern void TessBaseAPISetRectangle(IntPtr handle, int x, int y, int width, int height);

            [DllImport(DllPath, CharSet = CharSet.Auto, SetLastError = true)]
            public static extern void TessBaseAPISetPageSegMode(IntPtr handle, int mode);

            [DllImport(DllPath, CharSet = CharSet.Auto, SetLastError = true)]
            public static extern bool TessBaseAPISetVariable(IntPtr handle, [MarshalAs(UnmanagedType.LPStr)] string key, [MarshalAs(UnmanagedType.LPStr)] string value);

            [DllImport(DllPath, CharSet = CharSet.Auto, SetLastError = true)]
            public static extern void TessBaseAPIReadConfigFile(IntPtr handle, [MarshalAs(UnmanagedType.LPStr)] string file);
        }
    }
}
