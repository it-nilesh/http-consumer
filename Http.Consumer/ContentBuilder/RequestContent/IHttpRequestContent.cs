using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Http.Consumer.RequestContent
{
    public interface IHttpRequestContent : IDisposable
    {
        void AddFiles(List<HttpRequestFileContent> files);
        Task CopyToAsync(Stream stream);
        HttpContentHeaders Headers { get; }
        Task<byte[]> ReadAsByteArrayAsync();
    }
}
