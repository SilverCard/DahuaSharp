using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DahuaSharp.Packets
{
    public abstract class BinaryPacket
    {
        public byte Header { get; private set; }

        protected BinaryPacket(byte header)
        {
            Header = header;
        }

        public void Serialize(Stream stream)
        {
            BinarySerializer.Serialize(this, stream);
        }
    }
}
