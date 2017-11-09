using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DahuaSharp.Packets
{
    public class CaptureResponse : BinaryPacket
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
