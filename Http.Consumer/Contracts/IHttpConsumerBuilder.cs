using Http.Consumer.Exceptions;
using System;
using System.Threading.Tasks;

namespace Http.Consumer.Contracts
{
    public interface IHttpNextRequest
    {
        IHttpAggregateResources Next();
    }

    public interface IHttpConsumerBuilder : IHttpNextRequest
    {
        Task BuildAsync(Action<HttpReponseException> ex = null);
    }

    public interface IHttpConsumerBuilder<T> : IHttpNextRequest
    {
        Task<T> BuildAsync(Action<HttpReponseException> ex = null);
    }

    internal interface IHttpConsumerExecute
    {
        Task<object> Execute();
    }
}
