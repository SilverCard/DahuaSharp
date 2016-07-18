using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallDahuaLib.Packets
{
    public class LoginResponse : BinaryPacket
    {
        [Field(1)]
        public byte MultipleWindowsPreviewSupport { get; set; }

        [Field(2, 6)]
        public byte[] Unk1 { get; set; }

        [Field(3)]
        public byte ReturnCode { get; set; }
        [Field(4)]
        public byte Unk2 { get; set; }

        [Field(5)]
        public byte NumberChannels { get; set; }

        [Field(6)]
        public byte Encoder { get; set; }

        [Field(7)]
        public byte DevType { get; set; }

        [Field(8)]
        public byte DevSubType { get; set; }

        [Field(9, 2)]
        public byte[] Unk9 { get; set; }

        [Field(10)]
        public int LoginId { get; set; }

        [Field(11, 8)]
        public byte[] Unk11 { get; set; }

        [Field(12)]
        public byte VideoType { get; set; }

        [Field(13, 3)]
        public byte[] Unk13 { get; set; }

        public LoginResponse() : base(0xb0)
        {
        }
    }
}
