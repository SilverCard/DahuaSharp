using System;
using System.IO;

namespace DahuaSharp
{
    public static class Extentions
    {
        /// <summary>
        /// Read a response from stream with specified size.
        /// </summary>
        /// <param name="size">Expected size of response.</param>
        /// <returns></returns>
        public static Byte[] ReadResponse(this Stream stream, int size)
        {
            if (size <= 0) throw new ArgumentOutOfRangeException(nameof(size));

            Byte[] b = new byte[size];
            int i = 0;
            while (i < size)
            {
                int r = stream.Read(b, i, size - i);
                if (r == 0) break;
                i += r;
            }

            if (i != size) throw new ProtocolException($"Response have an unexpected size {i} bytes, expected {size}.");

            return b;
        }
    }
}
