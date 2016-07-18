using SmallDahuaLib.Packets;
using System;
using System.Net.Sockets;

namespace SmallDahuaLib
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

            if(response.ReturnCode != 0)
            {
                throw new LoginException(response.ReturnCode);
            }

            _LoginId = response.LoginId;
        }

        public byte[] CaptureChannel(byte channel)
        {
            var request = new CaptureRequest(channel);
            request.Serialize(_NStream);
            var response = BinarySerializer.Deserialize<CaptureResponse>(_NStream);
            byte[] bytes = _NStream.ReadAllBytes(response.Length);
            response = BinarySerializer.Deserialize<CaptureResponse>(_NStream);
            return bytes;
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
