using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilverCard.DahuaSharp.Packets
{
    public abstract class PacketBase
    {
        public byte Id { get; private set; }
        public byte[] Params { get; private set; }
        public byte[] Body { get; set; }

        public PacketBase(byte id)
        {
            Id = id;
            Params = new byte[3];
        }
    }
}
