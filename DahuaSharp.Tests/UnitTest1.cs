using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using SmallDahuaLib.Packets;
using System.Threading.Tasks;
using SilverCard.DahuaSharp.Packets;

namespace SmallDahuaLib.Tests
{
    [TestClass]
    public class UnitTest1
    {
        private void TestPacketSerialization(PacketBase pb, byte[] expected)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinarySerializer bs = new BinarySerializer(ms);
                bs.SerializeAsync(pb).Wait();
                ms.Position = 0;
                CollectionAssert.AreEqual(expected, ms.ToArray());
            }
        }

        private T TestPacketDeserialization<T>(byte[] packetBytes) where T : PacketBase, new()
        {
            using (MemoryStream ms = new MemoryStream(packetBytes))
            {
                ms.Position = 0;
                BinarySerializer bs = new BinarySerializer(ms);
                return bs.DeserializeAsync<T>().Result;  
   
            }
        }

        [TestMethod]
        public void LoginRequest()
        {

            byte[] packet = { 0xa0, 0x01, 0x00, 0x60, 0x00, 0x00, 0x00, 0x00,
                              0x61, 0x64, 0x6d, 0x69, 0x6e, 0x00, 0x00, 0x00,
                              0x61, 0x64, 0x6d, 0x69, 0x6e, 0x00, 0x00, 0x00,
                              0x05, 0x01, 0x00, 0x00, 0x00, 0x00, 0xa1, 0xaa };

            LoginRequest l = new LoginRequest("admin", "admin");
            TestPacketSerialization(l, packet);
        }

        [TestMethod]
        public void LoginResponse()
        {

            byte[] packet = { 
                            0xb0, 0x00, 0x00, 0x58, 0x00, 0x00, 0x00, 0x00,
                            0x00, 0x08, 0x05, 0x08, 0x3e, 0x00, 0x00, 0x00,
                            0x4c, 0xb0, 0x96, 0x0f, 0x01, 0x00, 0x00, 0x00,
                            0x06, 0x00, 0xf9, 0x00, 0x01, 0x04, 0x64, 0x02 };

            var resp = TestPacketDeserialization<LoginResponse>(packet);
     
        }
    }
}
