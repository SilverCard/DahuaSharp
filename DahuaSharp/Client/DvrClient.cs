using SmallDahuaLib.Packets;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SmallDahuaLib
{
    public class DvrClient
    {
        public String Host { get; private set; }
        public int Port { get; private set; }

        private TcpClient _Client;
        private NetworkStream _NStream;
        private BinarySerializer _Serializer;
        private int _LoginId;

        public DvrClient(String host, int port = 37777)
        {
            Host = host;
            Port = port;
            _Client = new TcpClient();
            Timeout = 30 * 1000;
        }

        public void Connect()
        {
           _Client.Connect(Host, Port);
            _NStream = _Client.GetStream();
            _Serializer = new BinarySerializer(_NStream);
        }

        public async Task LoginAsync(String username, String password)
        {
            LoginRequest packet = new LoginRequest(username, password);
            await _Serializer.SerializeAsync(packet);
            var response = await _Serializer.DeserializeAsync<LoginResponse>();

            if (response.ReturnCode != 0)
            {
                throw new LoginException(response.ReturnCode);
            }

            _LoginId = response.LoginId;
        }

        private static void Combine(ref byte[] first, byte[] second)
        {
            int l1, l2;
            l1 = first.Length;
            l2 = second.Length;
            Array.Resize(ref first, l1 + l2);
            Array.Copy(second, 0, first, l1, l2);
        }

        //public String[] GetChannelNames()
        //{
        //    byte[] request = new byte[32];
        //    request[0] = 0xa8;
        //    request[8] = 8;

        //    _NStream.Write(request, 0, 32);
        //    var response = _NStream.ReadAllBytes(32);
        //    int len = BitConverter.ToInt32(response, 4);
        //    var extraData = _NStream.ReadAllBytes(len);

        //    String longStr = Encoding.UTF8.GetString(extraData);
        //    return longStr.Split(new String[] { "&&" }, StringSplitOptions.RemoveEmptyEntries);
        //}

        [Obsolete("CaptureChannel is deprecated, please use TakeScreenshot instead.")]
        public byte[] CaptureChannel(byte channel)
        {
            return TakeScreenshot(channel);
        }

        /// <summary>
        /// Take a screenshot of a channel.
        /// </summary>
        /// <param name="channel">Channel number.</param>
        /// <returns>Image data.</returns>
        public byte[] TakeScreenshot(byte channel)
        {
            //var request = new CaptureRequest(channel);
            //request.Serialize(_NStream);
            //CaptureResponse response;
            //byte[] imageData = new byte[0];

            //do
            //{
            //    response = BinarySerializer.Deserialize<CaptureResponse>(_NStream);
            //    var bytes = _NStream.ReadAllBytes(response.Length);
            //    Combine(ref imageData, bytes);

            //} while (response.Length > 0);

            return null;
            //return imageData;
        }

        public void Logout()
        {
            //var request = new Logout() { LoginId = _LoginId };
            //request.Serialize(_NStream);
            //_Client.Close();
        }

        /// <summary>
        /// Timeout milliseconds, default is 30 seconds;
        /// </summary>
        public int Timeout
        {
            get
            {
                return _Client.ReceiveTimeout;
            }
            set
            {
                _Client.ReceiveTimeout = value;
                _Client.SendTimeout = value;
            }
        }
    }
}
