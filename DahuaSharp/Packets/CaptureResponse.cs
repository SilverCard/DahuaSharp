using SilverCard.DahuaSharp.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallDahuaLib.Packets
{
    public class CaptureResponse : PacketBase
    {

        [Field(0, 3)]
        public byte[] HeaderData { get; set; }

        [Field(1)]
        public int Length { get; set; }

        [Field(2, 24)]
        public byte[] Footer { get; set; }


        public CaptureResponse() : base(0xbc)
        {
        }
    }
}
