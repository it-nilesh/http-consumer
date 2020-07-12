using System;
using System.Collections.Concurrent;

namespace Http.Consumer
{
    using Http.Consumer.Contracts;
    using Http.Consumer.Factories;

    public class HttpConsumer : Authenticate, IHttpConsumer
    {
        public Uri Uri { get; private set; }

        public HttpConsumer()
        {
            HttpExecute = new ConcurrentBag<IHttpConsumerExecute>();
        }

        internal ConcurrentBag<IHttpConsumerExecute> HttpExecute { get; }
      
        internal ConcurrentBag<ISerializer> Serializers { get; private set; } = new ConcurrentBag<ISerializer>();
        
        internal ConcurrentBag<IDeserializer> Deserializers { get; private set; } = new ConcurrentBag<IDeserializer>();

        internal IHttpConsumer AddHttpRequest(IHttpConsumerExecute consumerExecute)
        {
            HttpExecute.Add(consumerExecute);
            return this;
        }

        internal IHttpConsumer ClearHttpRequest()
        {
            while (!HttpExecute.IsEmpty)
            {
                HttpExecute.TryTake(out IHttpConsumerExecute _);
            }

            return this;
        }

        public HttpConsumer(string host) : this()
        {
            Uri = new Uri(host);
        }

        public IHttpConsumer Host(string host)
        {
            Uri = new Uri(host);
            
            return this;
        }

        public IHttpRequest Resource(string resource)
        {
            return Resource(resource, null);
        }

        public IHttpRequest Resource(string resource, Action<HttpHeader> httpHeaderOptions)
        {
            var request = HttpWebRequestFactory.Create(new Uri(Uri, resource));
            var httpHeader = new HttpHeader(request, this);
            httpHeaderOptions?.Invoke(httpHeader);
            return new HttpRequest(request, this);
        }
       
        public IHttpConsumer AddSerializer(params ISerializer[] serializers)
        {
            foreach (ISerializer serializer in serializers)
            {
                Serializers.Add(serializer);
            }

            return this;
        }

        public IHttpConsumer AddDeserializer(params IDeserializer[] deserializers)
        {
            foreach (IDeserializer deserializer in deserializers)
            {
                Deserializers.Add(deserializer);
            }

            return this;
        }
    }
}
