﻿using SilverCard.DahuaSharp.Packets;
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


        [Field(1)]
        public byte Channel
        {
            get
            {
                return _Footer[0];
            }
            private set
            {
                _Footer[0] = value;
            }
        }

        [Field(2)]
        private byte[] _Footer = {
                                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                                0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00,
                                0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };


        public CaptureRequest() : base(0x11) { }

        public CaptureRequest(byte channel) : base(0x11)
        {

            Channel = channel;
        }
    }
}
