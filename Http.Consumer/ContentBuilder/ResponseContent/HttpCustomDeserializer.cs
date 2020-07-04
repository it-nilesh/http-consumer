using Http.Consumer.Contracts;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Http.Consumer.ResponseContent
{
    public class HttpCustomDeserializer : IHttpResponseContent
    {
        private readonly IDeserializer _deserializer;
        private readonly Stream _stream;

        public HttpCustomDeserializer(IDeserializer deserializer, Stream stream)
        {
            _deserializer = deserializer;
            _stream = stream;
        }

        public Task<T> DeserializeAsync<T>()
        {
            if (_stream.CanSeek)
            {
                _stream.Seek(0, SeekOrigin.Begin);
            }

            return _deserializer.DeserializeAsync<T>(_stream);
        }

        public void Dispose()
        {
            _stream?.Flush();
            _stream?.Dispose();

            _deserializer.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
