using SilverCard.DahuaSharp.Packets;
using System;

namespace SmallDahuaLib.Packets
{
    public class ConnectionRelationshipRequest : PacketBase
    {      
                
        [Field(0)]
        public int LoginId { get; private set; }

        [Field(1)]
        public byte Type { get; private set; }

        [Field(2)]
        public byte Channel { get; private set; }

        [Field(3)]
        private byte[] Footer;        

        public ConnectionRelationshipRequest(int loginId, byte type, byte channel, byte mode) : base(0xf1)
        {
            LoginId = loginId;
            Type = type;
            Channel = (byte)(channel + 1);
            //Mode = mode;
            Footer = new byte[18];
        }
    }
}
