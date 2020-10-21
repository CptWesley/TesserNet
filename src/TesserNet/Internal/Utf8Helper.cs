using System;
using System.Runtime.InteropServices;
using System.Text;

namespace TesserNet.Internal
{
    /// <summary>
    /// Provides classes to help with dealing with UTF8 strings.
    /// </summary>
    internal static class Utf8Helper
    {
        /// <summary>
        /// Reads a UTF8 string from a pointer.
        /// </summary>
        /// <param name="ptr">The pointer to read from.</param>
        /// <returns>The string at the pointer.</returns>
        public static string ToUtf8String(this IntPtr ptr)
        {
            byte[] bytes = new byte[ptr.GetStringLength()];

            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Marshal.ReadByte(ptr, i);
            }

            return Encoding.UTF8.GetString(bytes);
        }

        private static int GetStringLength(this IntPtr ptr)
        {
            int length = 0;
            while (true)
            {
                byte b = Marshal.ReadByte(ptr, length);
                if (b == 0)
                {
                    return length;
                }

                length++;
            }
        }
    }
}
