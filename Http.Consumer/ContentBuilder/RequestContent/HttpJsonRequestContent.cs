using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace Http.Consumer.RequestContent
{
    //TODO: File convert base64 
    //TODO: After convert Stream 
    public class HttpJsonRequestContent : HttpContent, IHttpRequestContent
    {
        private readonly MemoryStream _stream = new MemoryStream();
        private readonly JsonSerializerOptions _options = new JsonSerializerOptions();

        public HttpJsonRequestContent(object value)
        {
            _options.IgnoreNullValues = true;
            _options.IgnoreReadOnlyProperties = true;
            _options.PropertyNameCaseInsensitive = true;

            JsonSerializer.Serialize(new Utf8JsonWriter(_stream), value, value.GetType(), _options);
            _stream.Position = 0;

            Headers.ContentType = new MediaTypeHeaderValue(ContentType.Json);
        }

        protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            _stream.CopyTo(stream);
            var tcs = new TaskCompletionSource<object>();
            tcs.SetResult(null);
            return tcs.Task;
        }

        protected override bool TryComputeLength(out long length)
        {
            length = _stream.Length;
            return true;
        }

        protected override void Dispose(bool disposing)
        {
            _stream?.Flush();
            _stream?.Dispose();
            base.Dispose(disposing);
        }

        public void AddFiles(List<HttpRequestFileContent> files)
        {
        }
    }
}
