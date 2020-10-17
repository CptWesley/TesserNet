using System;
using System.Collections.Generic;
using System.IO;
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
        /// Loads the correct libraries into the runtime.
        /// </summary>
        internal static void Load()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string version = assembly.GetName().Version.ToString();
            string[] resources = assembly.GetManifestResourceNames();

            IEnumerable<string> files;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                if (Environment.Is64BitOperatingSystem)
                {
                    files = resources.Where(x => x.StartsWith("TesserNet.Resources.w64.", StringComparison.InvariantCulture));
                }
                else
                {
                    files = resources.Where(x => x.StartsWith("TesserNet.Resources.w86.", StringComparison.InvariantCulture));
                }
            }
            else
            {
                throw new PlatformNotSupportedException();
            }

            EnsureCopied(assembly, files);
        }

        private static void EnsureCopied(Assembly assembly, IEnumerable<string> files)
        {
            string tempRoot = Path.Combine(Path.GetTempPath(), "tessernet", assembly.GetName().Version.ToString());
            Directory.CreateDirectory(tempRoot);

            foreach (string file in files)
            {
                CopyResource(assembly, tempRoot, file);
            }
        }

        private static void CopyResource(Assembly assembly, string path, string resource)
        {
            string fileName = string.Join(".", resource.Split('.').Skip(3));
            string filePath = Path.Combine(path, fileName);

            if (!File.Exists(filePath))
            {
                using (FileStream fs = File.Create(filePath))
                {
                    using (Stream s = assembly.GetManifestResourceStream(resource))
                    {
                        s.CopyTo(fs);
                        s.Flush();
                    }
                }
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                NativeMethods.WindowsLoadLib(filePath);
            }
            else
            {
                NativeMethods.UnixLoadLib(filePath);
            }
        }

        private class NativeMethods
        {
            [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = false, SetLastError = true, EntryPoint = "LoadLibrary")]
            public static extern IntPtr WindowsLoadLib([MarshalAs(UnmanagedType.LPStr)] string lpFileName);

            [DllImport("libdl", CharSet = CharSet.Ansi, ExactSpelling = false, SetLastError = true, EntryPoint = "dlopen")]
            public static extern IntPtr UnixLoadLib([MarshalAs(UnmanagedType.LPStr)] string filename, int flags = 2);
        }
    }
}
