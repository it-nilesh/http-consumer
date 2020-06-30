using System.Net;

namespace Http.Consumer
{
    public sealed class LocalIPEndPoint
    {
        public IPEndPoint EndPoint { get; private set; }

        public void SetIPEndPoint(long address, int port = 0)
        {
            EndPoint = new IPEndPoint(address, port);
        }

        public void SetIPEndPoint(IPAddress address, int port = 0)
        {
            EndPoint = new IPEndPoint(address, port);
        }

        public EndPoint Create(SocketAddress socketAddress)
        {
            return EndPoint.Create(socketAddress);
        }
    }
}
