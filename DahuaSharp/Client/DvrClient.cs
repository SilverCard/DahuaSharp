using DahuaSharp.Packets;
using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace DahuaSharp
{
    public class DvrClient
    {
        public String Host { get; private set; }
        public int Port { get; private set; }

        private TcpClient _Client;
        private NetworkStream _NStream;
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
        }

        public void Login(String username, String password)
        {
            Login packet = new Login(username, password);
            packet.Serialize(_NStream);

            var response = BinarySerializer.Deserialize<LoginResponse>(_NStream);
            if(response.ReturnCode != 0) throw new LoginException(response.ReturnCode);

            _LoginId = response.LoginId;
        }

        public String[] GetChannelsTitles()
        {
            byte[] request = new byte[32];
            request[0] = 0xa8;
            request[8] = 8;

            _NStream.Write(request, 0, 32);
            var response = _NStream.ReadResponse(32);
            int len = BitConverter.ToInt32(response, 4);
            var extraData = _NStream.ReadResponse(len);

            String longStr = Encoding.UTF8.GetString(extraData);
            return longStr.Split(new String[] { "&&" }, StringSplitOptions.RemoveEmptyEntries);
        }

        public byte[] GetSnapshot(byte channel = 0)
        {
            var request = new CaptureRequest(channel);
            request.Serialize(_NStream);
            CaptureResponse response;   

            using (MemoryStream ms = new MemoryStream())
            {
                do
                {
                    response = BinarySerializer.Deserialize<CaptureResponse>(_NStream);
                    var bytes = _NStream.ReadResponse(response.Length);
                    if(bytes.Length > 0)
                    {
                        ms.Write(bytes, 0, bytes.Length);
                    }

                } while (response.Length > 0);

                return ms.ToArray();
            }
        }

        public void Logout()
        {
            var request = new Logout() { LoginId = _LoginId };
            request.Serialize(_NStream);
            _Client.Close();
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
