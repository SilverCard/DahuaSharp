﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DahuaSharp.Packets
{
    public class Logout : BinaryPacket
    {
        [Field(0)]
        private byte[] HeaderData = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, };

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
