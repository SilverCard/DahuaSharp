using SilverCard.DahuaSharp.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallDahuaLib.Packets
{
    internal class CaptureRequest : PacketBase
    {
        [Field(0)]
        private static readonly byte[] HeaderData = {
                                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                                0x00, 0x00, 0x00, 0x00, 0x0a, 0x00, 0x00, 0x00 };


        public byte Channel
        {
            get
            {
                return Body[0];
            }
            private set
            {
                Body[0] = value;
            }
        }


        public CaptureRequest() : base(0x11) { }

        public CaptureRequest(byte channel) : base(0x11)
        {
            Body = new byte[] {
                                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                                0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00,
                                0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            Channel = channel;

        }
    }
}
