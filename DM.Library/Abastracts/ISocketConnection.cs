using System.Net;

namespace DM.Library
{
    public interface ISocketConnection
    {
        bool IsConnection { get; set; }

        IPEndPoint EndPoint { get; set; }

        void Setup(string ip, int port, int maximumReady);

        void Send(string msg);
    }
}
