using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace TesserNet.Internal
{
    /// <summary>
    /// Windows implementation of the Tesseract API.
    /// </summary>
    /// <seealso cref="TesseractApi" />
    internal class WindowsTesseractApi : TesseractApi
    {
        /// <inheritdoc/>
        [SupportedOSPlatform(PlatformNames.Windows)]
        public override IntPtr TessBaseAPICreate()
            => NativeMethods.TessBaseAPICreate();

        /// <inheritdoc/>
        [SupportedOSPlatform(PlatformNames.Windows)]
        public override void TessBaseAPIDelete(IntPtr handle)
            => NativeMethods.TessBaseAPIDelete(handle);

        /// <inheritdoc/>
        [SupportedOSPlatform(PlatformNames.Windows)]
        public override string TessBaseAPIGetUTF8Text(IntPtr handle)
            => NativeMethods.TessBaseAPIGetUTF8Text(handle).ToUtf8String();

        /// <inheritdoc/>
        [SupportedOSPlatform(PlatformNames.Windows)]
        public override int TessBaseAPIInit1(IntPtr handle, string dataPath, string language, int oem, IntPtr configs, int configSize)
            => NativeMethods.TessBaseAPIInit1(handle, dataPath, language, oem, configs, configSize);

        /// <inheritdoc/>
        [SupportedOSPlatform(PlatformNames.Windows)]
        public override void TessBaseAPISetImage(IntPtr handle, IntPtr data, int width, int height, int bytesPerPixel, int bytesPerLine)
            => NativeMethods.TessBaseAPISetImage(handle, data, width, height, bytesPerPixel, bytesPerLine);

        /// <inheritdoc/>
        [SupportedOSPlatform(PlatformNames.Windows)]
        public override void TessBaseAPISetSourceResolution(IntPtr handle, int ppi)
            => NativeMethods.TessBaseAPISetSourceResolution(handle, ppi);

        /// <inheritdoc/>
        [SupportedOSPlatform(PlatformNames.Windows)]
        public override void TessBaseAPISetRectangle(IntPtr handle, int x, int y, int width, int height)
            => NativeMethods.TessBaseAPISetRectangle(handle, x, y, width, height);

        /// <inheritdoc/>
        [SupportedOSPlatform(PlatformNames.Windows)]
        public override void TessBaseAPIClear(IntPtr handle)
            => NativeMethods.TessBaseAPIClear(handle);

        /// <inheritdoc/>
        [SupportedOSPlatform(PlatformNames.Windows)]
        public override void TessBaseAPISetPageSegMode(IntPtr handle, int mode)
            => NativeMethods.TessBaseAPISetPageSegMode(handle, mode);

        /// <inheritdoc/>
        [SupportedOSPlatform(PlatformNames.Windows)]
        public override bool TessBaseAPISetVariable(IntPtr handle, string key, string value)
            => NativeMethods.TessBaseAPISetVariable(handle, key, value);

        /// <inheritdoc/>
        [SupportedOSPlatform(PlatformNames.Windows)]
        public override void TessBaseAPIReadConfigFile(IntPtr handle, string file)
            => NativeMethods.TessBaseAPIReadConfigFile(handle, file);

        private static class NativeMethods
        {
            private const string DllPath = "libtesseract500";

            [SupportedOSPlatform(PlatformNames.Windows)]
            [DllImport(DllPath, CharSet = CharSet.Auto, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr TessBaseAPICreate();

            [SupportedOSPlatform(PlatformNames.Windows)]
            [DllImport(DllPath, CharSet = CharSet.Auto, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TessBaseAPIDelete(IntPtr handle);

            [SupportedOSPlatform(PlatformNames.Windows)]
            [DllImport(DllPath, CharSet = CharSet.Auto, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TessBaseAPIClear(IntPtr handle);

            [SupportedOSPlatform(PlatformNames.Windows)]
            [DllImport(DllPath, CharSet = CharSet.Auto, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
            public static extern int TessBaseAPIInit1(IntPtr handle, [MarshalAs(UnmanagedType.LPStr)] string dataPath, [MarshalAs(UnmanagedType.LPStr)] string language, int oem, IntPtr configs, int configSize);

            [SupportedOSPlatform(PlatformNames.Windows)]
            [DllImport(DllPath, CharSet = CharSet.Auto, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TessBaseAPISetImage(IntPtr handle, IntPtr data, int width, int height, int bytesPerPixel, int bytesPerLine);

            [SupportedOSPlatform(PlatformNames.Windows)]
            [DllImport(DllPath, CharSet = CharSet.Auto, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr TessBaseAPIGetUTF8Text(IntPtr handle);

            [SupportedOSPlatform(PlatformNames.Windows)]
            [DllImport(DllPath, CharSet = CharSet.Auto, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TessBaseAPISetSourceResolution(IntPtr handle, int ppi);

            [SupportedOSPlatform(PlatformNames.Windows)]
            [DllImport(DllPath, CharSet = CharSet.Auto, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TessBaseAPISetRectangle(IntPtr handle, int x, int y, int width, int height);

            [SupportedOSPlatform(PlatformNames.Windows)]
            [DllImport(DllPath, CharSet = CharSet.Auto, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TessBaseAPISetPageSegMode(IntPtr handle, int mode);

            [SupportedOSPlatform(PlatformNames.Windows)]
            [DllImport(DllPath, CharSet = CharSet.Auto, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool TessBaseAPISetVariable(IntPtr handle, [MarshalAs(UnmanagedType.LPStr)] string key, [MarshalAs(UnmanagedType.LPStr)] string value);

            [SupportedOSPlatform(PlatformNames.Windows)]
            [DllImport(DllPath, CharSet = CharSet.Auto, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TessBaseAPIReadConfigFile(IntPtr handle, [MarshalAs(UnmanagedType.LPStr)] string file);
        }
    }
}
