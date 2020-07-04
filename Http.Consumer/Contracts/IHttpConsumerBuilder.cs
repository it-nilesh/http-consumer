using Http.Consumer.Exceptions;
using System;
using System.Threading.Tasks;

namespace Http.Consumer.Contracts
{
    public interface IHttpNextRequest
    {
        IHttpConsumer Next();
        IHttpRequest Next(string host, string resource, Action<HttpHeader> httpHeaderOptions = null);
        IHttpRequest Next(string resource, Action<HttpHeader> httpHeaderOptions = null);
    }

    public interface IHttpConsumerBuilder : IHttpNextRequest
    {
        Task BuildAsync(Action<HttpReponseException> ex = null);
       
        Task<IHttpResponse> BuildWithHeaderAsync(Action<HttpReponseException> ex = null);
    }

    public interface IHttpConsumerBuilder<T> : IHttpNextRequest, IHttpAggregateResources
    {
        Task<T> BuildAsync(Action<HttpReponseException> ex = null);
        
        Task<IHttpResponse<T>> BuildWithHeaderAsync(Action<HttpReponseException> ex = null);
    }

    internal interface IHttpConsumerExecute
    {
        Task<object> Execute();
    }

    public interface IHttpResponse
    {
        HeaderDictionary RequestHeader { get; }

        HeaderDictionary ResponseHeader { get; }
    }

    public interface IHttpResponse<T> : IHttpResponse
    {
        T Content { get; }
    }
}
