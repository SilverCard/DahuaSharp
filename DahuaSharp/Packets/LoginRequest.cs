using SilverCard.DahuaSharp.Packets;
using System;

namespace SmallDahuaLib.Packets
{
    public class LoginRequest : PacketBase
    {      
                
        [Field(1,8)]
        public String Username { get; private set; }

        [Field(2, 8)]
        public String Password { get; private set; }

        [Field(3)]
        private byte[] Footer = new byte[] { 0x05, 0x01, 0x00, 0x00, 0x00, 0x00, 0xa1, 0xaa };
        

        public LoginRequest(String username, String password) : base(0xa0)
        {
            Username = username;
            Password = password;
            Params[0] = 1;
            Params[2] = 0x60;
        }
    }
}
