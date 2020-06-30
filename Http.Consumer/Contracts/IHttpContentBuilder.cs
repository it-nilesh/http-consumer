using Http.Consumer.RequestContent;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Http.Consumer.Contracts
{
    public interface IHttpContentBuilder
    {
        Task<TResult> HttpResponseAsync<TResult>();
        Task<TResult> HttpResponseAsync<TResult>(HttpRequestContent<object> payload);
        Task<Stream> ReceiveFileAsync();
        Task<HttpWebResponse> ExecuteAsync(HttpRequestContent<object> payload);
    }
}
