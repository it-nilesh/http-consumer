using System;
using System.IO;
using System.Threading.Tasks;

namespace Http.Consumer
{
    public interface IContentInfo : IDisposable
    {
        string ContentType { get; }
    }

    public interface ISerializer : IContentInfo
    {
      Task<Stream> SerializeAsync(object value);
    }

    public interface IDeserializer : IContentInfo
    {
        Task<T> DeserializeAsync<T>(Stream stream);
    }
}
