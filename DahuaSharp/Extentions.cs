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
        public static Byte[] ReadAllBytes(this Stream stream, int count)
        {
            Byte[] b = new byte[count];
            int i = 0;
            while (i < count)
            {
                int r = stream.Read(b, i, count - i);
                i += r;
            }

            return b;
        }
    }
}
