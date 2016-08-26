using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SmallDahuaLib
{
    public static class Extentions
    {
        public static async Task<Byte[]> ReadAllBytesAsync(this Stream stream, int count, CancellationToken ct)
        {
            Byte[] b = new byte[count];
            int i = 0;
            while (i < count)
            {
                int r = await stream.ReadAsync(b, i, count - i, ct );

                if(r == 0)
                {
                    throw new Exception("Failed to read bytes from stream.");
                }

                i += r;
            }

            return b;
        }
    }
}
