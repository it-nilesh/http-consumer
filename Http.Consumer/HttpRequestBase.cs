using System;
using System.Threading.Tasks;

namespace Http.Consumer
{
    using Http.Consumer.Contracts;
    using Http.Consumer.RequestContent;
    using System.Net;

    public class HttpRequestBase
    {
        public HttpRequestBase(HttpWebRequest httpWebRequest, IHttpConsumer httpConsumer)
        {
            HttpWebRequest = httpWebRequest;
            HttpContent = new HttpContentBuilder(httpWebRequest);
            HttpConsumer = httpConsumer;
            ResourceUri = httpWebRequest.RequestUri;
        }

        protected virtual IHttpConsumer HttpConsumer { get; }
        protected virtual IHttpContentBuilder HttpContent { get; }
        protected virtual HttpWebRequest HttpWebRequest { get; }
        public Uri ResourceUri { get; }

        protected virtual IHttpConsumerBuilder<TResult> TransferDataBuilder<TResult>(object payload, Action<HttpRequestContent<object>> contentOptions)
        {
            return new HttpConsumerBuilder<TResult>(() => TransferDataBuilderResult<TResult>(payload, contentOptions), HttpConsumer);
        }

        protected virtual IHttpConsumerBuilder TransferDataBuilder(object payload, Action<HttpRequestContent<object>> contentOptions)
        {
            return new HttpConsumerBuilder<object>(() => TransferDataBuilderResult<object>(payload, contentOptions), HttpConsumer);
        }

        protected virtual async Task<TResult> TransferDataBuilderResult<TResult>(object payload, Action<HttpRequestContent<object>> contentOptions)
        {
            var requestContent = new HttpRequestContent<object>(payload);
            contentOptions?.Invoke(requestContent);
            return await HttpContent.HttpResponseAsync<TResult>(requestContent);
        }

        protected virtual IHttpConsumerBuilder<TResult> RetriveDataBuilder<TResult>()
        {
            async Task<TResult> requestFunc()
            {
                return await HttpContent.HttpResponseAsync<TResult>();
            }

            return new HttpConsumerBuilder<TResult>(requestFunc, HttpConsumer);
        }

        protected virtual void PreRequestConfiguration(string httpMethod, object id = null, Action<HttpRequestQueryString> queryStringOptions = null)
        {
            //TODO: Move code to another file
            HttpWebRequest.Method = httpMethod;
            string url = GetQueryStringUrl(id, queryStringOptions);
            HttpWebRequest.CreateUri(ResourceUri, url);
        }

        protected virtual string GetQueryStringUrl(object id, Action<HttpRequestQueryString> queryStringOptions)
        {
            //TODO: Move code to another file
            string url = string.Empty;
            if (queryStringOptions != null)
            {
                var requestQueryString = new HttpRequestQueryString();
                queryStringOptions.Invoke(requestQueryString);
                url = requestQueryString.GetFullQueryString();
            }

            if (string.IsNullOrWhiteSpace(url))
                url = id?.ToString();
            else
                url = $"{id}?{url}";

            return url;
        }
    }
}
