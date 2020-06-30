using System;
using System.Threading.Tasks;

namespace Http.Consumer.ResponseContent
{
    public interface IHttpResponseContent : IDisposable
    {
        Task<T> DeserializeAsync<T>();
    }
}
