using System;
using System.IO;

namespace Http.Consumer.Contracts
{
    using Http.Consumer.RequestContent;

    public interface IHttpRequest
    {
        IHttpConsumerBuilder<TResult> Get<TResult>(Action<HttpRequestQueryString> queryString = null);

        IHttpConsumerBuilder<TResult> Get<TResult>(object id, Action<HttpRequestQueryString> queryString = null);

        IHttpConsumerBuilder Post(object payload, Action<HttpRequestQueryString> queryString = null, Action<HttpRequestContent<object>> contentOptions = null);

        IHttpConsumerBuilder<TResult> Post<TResult>(object payload, Action<HttpRequestQueryString> queryString = null, Action<HttpRequestContent<object>> contentOptions = null);

        IHttpConsumerBuilder Put(object id, object payload, Action<HttpRequestQueryString> queryString = null, Action<HttpRequestContent<object>> contentOptions = null);

        IHttpConsumerBuilder<TResult> Put<TResult>(object id, object payload, Action<HttpRequestQueryString> queryString = null, Action<HttpRequestContent<object>> contentOptions = null);

        IHttpConsumerBuilder<TResult> Delete<TResult>(object id, object payload = null, Action<HttpRequestQueryString> queryString = null);

        IHttpConsumerBuilder Delete(object id, object payload = null, Action<HttpRequestQueryString> queryString = null);

        IHttpConsumerBuilder<Stream> DownloadFile(object url, Action<HttpRequestQueryString> queryString = null);
    }
}
