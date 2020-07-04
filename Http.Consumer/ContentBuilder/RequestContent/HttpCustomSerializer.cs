using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Http.Consumer.RequestContent
{
    public class HttpCustomSerializer : HttpContent, IHttpRequestContent
    {
        private readonly ISerializer _serializer;
        private readonly object _value;

        public HttpCustomSerializer(ISerializer serializer, object value)
        {
            _serializer = serializer;
            _value = value;
            Headers.ContentType = new MediaTypeHeaderValue(_serializer.ContentType);
        }

        protected override async Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            Stream innerStream = await _serializer.SerializeAsync(_value);
            if (innerStream.CanSeek)
            {
                innerStream.Seek(0, SeekOrigin.Begin);
            }

            innerStream.CopyTo(stream);
        }

        protected override bool TryComputeLength(out long length)
        {
            length = -1;
            return true;
        }

        public void AddFiles(List<HttpRequestFileContent> files)
        {

        }

        protected override void Dispose(bool disposing)
        {
            _serializer?.Dispose();
            base.Dispose(disposing);

            GC.SuppressFinalize(this);
        }
    }
}
