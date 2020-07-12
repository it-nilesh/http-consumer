using System;

namespace Http.Consumer
{
    using Http.Consumer.Contracts;
    
    public interface IHttpConsumer : IAuthenticate
    {
        Uri Uri { get; }

        IHttpRequest Resource(string resource);

        IHttpRequest Resource(string resource, Action<HttpHeader> httpHeaderOptions);

        IHttpConsumer Host(string host);

        IHttpConsumer AddSerializer(params ISerializer[] serializers);

        IHttpConsumer AddDeserializer(params IDeserializer[] deserializers);
    }
}
