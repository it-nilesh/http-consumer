using Http.Consumer.RequestContent;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Http.Consumer.Contracts
{
    public interface IHttpContentBuilder
    {
        Task<IHttpResponse<TResult>> HttpResponseAsync<TResult>();
        Task<IHttpResponse<TResult>> HttpResponseAsync<TResult>(HttpRequestContent<object> payload);
        Task<IHttpResponse<Stream>> ReceiveFileAsync();
        Task<HttpWebResponse> ExecuteAsync(HttpRequestContent<object> payload);
    }
}
