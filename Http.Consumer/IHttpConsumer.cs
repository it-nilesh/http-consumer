using System;

namespace Http.Consumer
{
    using Http.Consumer.Contracts;
    
    public interface IHttpConsumer : IAuthenticate
    {
        IHttpRequest Resource(string resource);

        IHttpRequest Resource(string resource, Action<HttpHeader> httpHeaderOptions);

        IHttpConsumer Host(string host);

        IHttpConsumer Host(string host, Action<LocalIPEndPoint> ipEndPoint);
    }
}
