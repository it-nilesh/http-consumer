using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Http.Consumer.ResponseContent
{
    public class HttpJsonResponseContent : IHttpResponseContent
    {
        private readonly JsonSerializerOptions _options = new JsonSerializerOptions();

        private readonly Stream _stream;
        public HttpJsonResponseContent(Stream stream)
        {
            _options.IgnoreNullValues = true;
            _options.IgnoreReadOnlyProperties = true;
            _options.PropertyNameCaseInsensitive = true;
            _stream = stream;
        }

        public async Task<T> DeserializeAsync<T>()
        {
            if (_stream.CanSeek)
            {
                _stream.Seek(0, SeekOrigin.Begin);
            }

            return await JsonSerializer.DeserializeAsync<T>(_stream, _options);
        }

        public void Dispose()
        {
            _stream?.Flush();
            _stream?.Dispose();

            GC.SuppressFinalize(this);
        }
    }
}
