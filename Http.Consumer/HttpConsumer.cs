using System;
using System.Collections.Concurrent;

namespace Http.Consumer
{
    using Http.Consumer.Contracts;
    using Http.Consumer.Factories;

    public class HttpConsumer : Authenticate, IHttpConsumer
    {
        public Action<LocalIPEndPoint> IpEndPoint { get; private set; }

        public Uri Uri { get; private set; }

        public HttpConsumer()
        {
            HttpExecute = new ConcurrentBag<IHttpConsumerExecute>();
            base.HttpConsumer = this;
        }

        internal ConcurrentBag<IHttpConsumerExecute> HttpExecute { get; }

        internal IHttpConsumer SetWebRequest(IHttpConsumerExecute consumerExecute)
        {
            HttpExecute.Add(consumerExecute);
            return this;
        }

        public HttpConsumer(string host) : this()
        {
            Uri = new Uri(host);
        }

        public IHttpConsumer Host(string host, Action<LocalIPEndPoint> ipEndPoint)
        {
            Uri = new Uri(host);
            IpEndPoint = ipEndPoint;

            return this;
        }

        public IHttpConsumer Host(string host)
        {
            return Host(host, null);
        }

        public IHttpRequest Resource(string resource)
        {
            return Resource(resource, null);
        }

        public IHttpRequest Resource(string resource, Action<HttpHeader> httpHeaderOptions)
        {
            var request = HttpWebRequestFactory.Create(new Uri(Uri, resource));
            request.BindIPEndPoint(IpEndPoint);
            var httpHeader = new HttpHeader(request, this);
            httpHeaderOptions?.Invoke(httpHeader);

            return new HttpRequest(request, this);
        }
    }
}
