using SmallDahuaLib;

namespace SilverCard.DahuaSharp.Packets
{
    public class GetChannelNamesRequest : PacketBase
    {
        [Field(0)]
        private byte[] _Footer;

        public GetChannelNamesRequest() : base(0xa8)
        {
            _Footer = new byte[24];
            _Footer[0] = 8;
        }
    }
}
