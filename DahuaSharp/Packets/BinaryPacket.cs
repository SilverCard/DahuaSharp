namespace SmallDahuaLib.Packets
{
    public class BinaryPacket
    {
        public byte Id { get; set; }
        public byte[] Params { get; private set; }
        public byte[] Header { get; private set; }
        public byte[] Body { get; set; }

        public BinaryPacket()
        {
            Params = new byte[3];
            Header = new byte[24];
            Body = null;
        }

    }
}
