using SilverCard.DahuaSharp.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallDahuaLib.Packets
{
    public class Logout : PacketBase
    {

        [Field(1)]
        public int LoginId { get; set; }

        [Field(2)]
        private byte[] Footer = new byte[] {
                                            0x00, 0x00, 0x00, 0x00,
                                            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                                            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00};

        public Logout() : base(0x0a)
        {

        }       

    }
}
