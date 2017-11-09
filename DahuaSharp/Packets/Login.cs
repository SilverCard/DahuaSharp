using System;

namespace DahuaSharp.Packets
{
    public class Login : BinaryPacket
    {        
        [Field(0)]
        private byte[] Header1 = new byte[] { 0x01, 0x00, 0x60, 0x00, 0x00, 0x00, 0x00 };
        
        [Field(1,8)]
        public String Username { get; private set; }

        [Field(2, 8)]
        public String Password { get; private set; }

        [Field(3)]
        private byte[] Footer = new byte[] { 0x05, 0x01, 0x00, 0x00, 0x00, 0x00, 0xa1, 0xaa };
        

        public Login(String username, String password) : base(0xa0)
        {
            Username = username;
            Password = password;
        }
    }
}
