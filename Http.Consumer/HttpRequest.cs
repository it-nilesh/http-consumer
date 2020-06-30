using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Http.Consumer
{
    using Http.Consumer.Contracts;
    using Http.Consumer.RequestContent;
    using Utils;

    public class HttpRequest : HttpRequestBase, IHttpRequest
    {
        public HttpRequest(HttpWebRequest httpWebRequest, IHttpConsumer httpConsumer) :
            base(httpWebRequest, httpConsumer)
        { }

        public IHttpConsumerBuilder<TResult> Get<TResult>(Action<HttpRequestQueryString> queryString = null)
        {
            PreRequestConfiguration(HttpMethod.Get, queryStringOptions: queryString);
            return RetriveDataBuilder<TResult>();
        }

        public IHttpConsumerBuilder<TResult> Get<TResult>(object id, Action<HttpRequestQueryString> queryString = null)
        {
            PreRequestConfiguration(HttpMethod.Get, id, queryString);
            return RetriveDataBuilder<TResult>();
        }

        public IHttpConsumerBuilder Post(object payload, Action<HttpRequestQueryString> queryString = null, Action<HttpRequestContent<object>> contentOptions = null)
        {
            PreRequestConfiguration(HttpMethod.Post, queryStringOptions: queryString);
            return TransferDataBuilder(payload, contentOptions);
        }

        public IHttpConsumerBuilder<TResult> Post<TResult>(object payload, Action<HttpRequestQueryString> queryString = null, Action<HttpRequestContent<object>> contentOptions = null)
        {
            PreRequestConfiguration(HttpMethod.Post, queryStringOptions: queryString);
            return TransferDataBuilder<TResult>(payload, contentOptions);
        }

        public IHttpConsumerBuilder Put(object id, object payload, Action<HttpRequestQueryString> queryString = null, Action<HttpRequestContent<object>> contentOptions = null)
        {
            PreRequestConfiguration(HttpMethod.Put, id, queryString);
            return TransferDataBuilder(payload, contentOptions);
        }

        public IHttpConsumerBuilder<TResult> Put<TResult>(object id, object payload, Action<HttpRequestQueryString> queryString = null, Action<HttpRequestContent<object>> contentOptions = null)
        {
            PreRequestConfiguration(HttpMethod.Put, id, queryString);
            return TransferDataBuilder<TResult>(payload, contentOptions);
        }

        public IHttpConsumerBuilder Delete(object id, object payload = null, Action<HttpRequestQueryString> queryString = null)
        {
            PreRequestConfiguration(HttpMethod.Delete, id, queryString);
            return TransferDataBuilder(payload, null);
        }

        public IHttpConsumerBuilder<TResult> Delete<TResult>(object id, object payload = null, Action<HttpRequestQueryString> queryString = null)
        {
            PreRequestConfiguration(HttpMethod.Delete, id, queryString);
            return TransferDataBuilder<TResult>(payload, null);
        }

        public IHttpConsumerBuilder<Stream> DownloadFile(object url, Action<HttpRequestQueryString> queryString = null)
        {
            async Task<Stream> requestFunc()
            {
                HttpWebRequest.AllowReadStreamBuffering = false;
                PreRequestConfiguration(HttpMethod.Get, url, queryString);
                return await HttpContent.ReceiveFileAsync();
            }

            return new HttpConsumerBuilder<Stream>(requestFunc, HttpConsumer);
        }
    }
}
