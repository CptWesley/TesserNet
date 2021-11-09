using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace TesserNet.Internal
{
    /// <summary>
    /// Provides functionality for loading the correct libraries into the runtime.
    /// </summary>
    internal static class Loader
    {
        /// <summary>
        /// Gets the temporary directory to which the files were unpacked.
        /// </summary>
        /// <returns>The temporary unpack directory.</returns>
        internal static string GetUnpackDirectory()
            => Path.Combine(Path.GetTempPath(), "tessernet", Assembly.GetExecutingAssembly().GetName().Version.ToString());

        /// <summary>
        /// Loads the correct libraries into the runtime.
        /// </summary>
        internal static void Load()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Stream stream = assembly.GetManifestResourceStream("TesserNet.Resources.zip");
            ZipArchive resources = new ZipArchive(stream);

            IEnumerable<ZipArchiveEntry> files;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                if (Environment.Is64BitOperatingSystem)
                {
                    files = resources.ForPlatform("w64");
                }
                else
                {
                    files = resources.ForPlatform("w86");
                }
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                files = resources.ForPlatform("linux");
            }
            else
            {
                throw new PlatformNotSupportedException();
            }

            EnsureCopied(files);
            resources.Dispose();
            stream.Dispose();
        }

        private static void EnsureCopied(IEnumerable<ZipArchiveEntry> entries)
        {
            string tempRoot = GetUnpackDirectory();
            Directory.CreateDirectory(tempRoot);

            foreach (ZipArchiveEntry entry in entries)
            {
                CopyResource(tempRoot, entry);
            }
        }

        private static void CopyResource(string path, ZipArchiveEntry entry)
        {
            string fileName = Path.GetFileName(entry.FullName);
            string filePath = Path.Combine(path, fileName);

            if (!File.Exists(filePath))
            {
                entry.ExtractToFile(filePath, false);
            }

            string extension = Path.GetExtension(filePath);
            if (extension == ".dll" && RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                NativeMethods.WindowsLoadLib(filePath);
            }
            else if (extension == ".so" || extension == ".dylib")
            {
                NativeMethods.UnixLoadLib(filePath);
            }
        }

        private static IEnumerable<ZipArchiveEntry> ForPlatform(this ZipArchive resources, string platform)
            => resources.Entries.Where(x =>
            (x.FullName.StartsWith($"{platform}/", StringComparison.InvariantCulture) && x.FullName.Length > platform.Length + 1)
            || (x.FullName.StartsWith("any/", StringComparison.InvariantCulture) && x.FullName.Length > 4));

        private class NativeMethods
        {
            [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = false, SetLastError = true, EntryPoint = "LoadLibrary")]
            public static extern IntPtr WindowsLoadLib([MarshalAs(UnmanagedType.LPStr)] string lpFileName);

            [DllImport("libdl", CharSet = CharSet.Ansi, ExactSpelling = false, SetLastError = true, EntryPoint = "dlopen")]
            public static extern IntPtr UnixLoadLib([MarshalAs(UnmanagedType.LPStr)] string filename, int flags = 2);
        }
    }
}
