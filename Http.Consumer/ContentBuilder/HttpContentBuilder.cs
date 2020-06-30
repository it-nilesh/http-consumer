using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Http.Consumer.RequestContent
{
    using Http.Consumer.ResponseContent;
    using Http.Consumer.Contracts;
    using System;

    public class HttpContentBuilder : IHttpContentBuilder
    {
        private readonly HttpRequestDelegate _httpRequest;
        private readonly HttpResponseDelegate _httpResponse;

        public HttpContentBuilder(HttpWebRequest httpWebRequest)
        {
            _httpRequest = new HttpRequestDelegate(httpWebRequest);
            _httpResponse = new HttpResponseDelegate(httpWebRequest);
        }

        public async Task<TResult> HttpResponseAsync<TResult>()
        {
            return await _httpResponse.ExecuteAsync<TResult>();
        }

        public Task<Stream> ReceiveFileAsync()
        {
            return _httpResponse.ReceiveFileAsync();
        }


        public async Task<HttpWebResponse> ExecuteAsync(HttpRequestContent<object> payload)
        {
            await _httpRequest.ExecuteAsync(payload);
            var httpResponse = await _httpResponse.ExecuteAsync();
            _httpRequest.Dispose();
            return httpResponse;
        }

        public async Task<TResult> HttpResponseAsync<TResult>(HttpRequestContent<object> payload)
        {
            await _httpRequest.ExecuteAsync(payload);
            var responseContent = await _httpResponse.ExecuteAsync<TResult>();
            _httpRequest.Dispose();
            return responseContent;
        }
    }
}
