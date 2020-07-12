using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Http.Consumer.RequestContent
{
    using Http.Consumer.Contracts;
    using Http.Consumer.ResponseContent;
    using System.Collections.Generic;

    public class HttpContentBuilder : IHttpContentBuilder
    {
        private readonly HttpRequestDelegate _httpRequest;
        private readonly HttpResponseDelegate _httpResponse;

        public HttpContentBuilder(HttpWebRequest httpWebRequest, IReadOnlyCollection<ISerializer> serializers, IReadOnlyCollection<IDeserializer> deserializers)
        {
            _httpRequest = new HttpRequestDelegate(httpWebRequest, serializers);
            _httpResponse = new HttpResponseDelegate(httpWebRequest, deserializers);
        }

        public async Task<IHttpResponse<TResult>> HttpResponseAsync<TResult>()
        {
            return await _httpResponse.ExecuteAsync<TResult>();
        }

        public Task<IHttpResponse<Stream>> ReceiveFileAsync()
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

        public async Task<IHttpResponse<TResult>> HttpResponseAsync<TResult>(HttpRequestContent<object> payload)
        {
            await _httpRequest.ExecuteAsync(payload);
            var response = await _httpResponse.ExecuteAsync<TResult>();
            _httpRequest.Dispose();
            return response;
        }
    }
}
