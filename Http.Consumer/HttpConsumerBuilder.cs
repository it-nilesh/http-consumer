using System;
using System.Threading.Tasks;

namespace Http.Consumer
{
    using Http.Consumer.Contracts;
    using Http.Consumer.Exceptions;
    using System.Collections.Generic;
    using System.Net;

    public class HttpConsumerBuilder<T> : IHttpConsumerBuilder<T>, IHttpConsumerBuilder, IHttpConsumerExecute
    {
        private readonly Func<Task<IHttpResponse<T>>> _httpExecute;
        private readonly Func<HttpAggregateResult<T>, T> _aggregate;
        private readonly IHttpConsumer _httpConsumer;

        public HttpConsumerBuilder(Func<Task<IHttpResponse<T>>> httpExecute, IHttpConsumer httpConsumer)
        {
            _httpExecute = httpExecute;
            _httpConsumer = httpConsumer;
        }

        public HttpConsumerBuilder(Func<HttpAggregateResult<T>, T> aggregate, IHttpConsumer httpConsume)
        {
            _aggregate = aggregate;
            _httpConsumer = httpConsume;
        }

        public IHttpConsumerBuilder<TAggregate> Aggregate<TAggregate>(Action<HttpAggregateResult<TAggregate>, TAggregate> result) where TAggregate : new()
        {
            ((HttpConsumer)_httpConsumer).AddHttpRequest(this);

            return new HttpAggregateResources(_httpConsumer)
                       .Aggregate<TAggregate>(result);
        }

        public IHttpConsumer Next()
        {
            ((HttpConsumer)_httpConsumer).AddHttpRequest(this);
            return _httpConsumer;
        }

        public IHttpRequest Next(string host, string resource, Action<HttpHeader> httpHeaderOptions = null)
        {
            _httpConsumer.Host(host);
            return Next(resource, httpHeaderOptions);
        }

        public IHttpRequest Next(string resource, Action<HttpHeader> httpHeaderOptions = null)
        {
            ((HttpConsumer)_httpConsumer).AddHttpRequest(this);
            return _httpConsumer.Resource(resource, httpHeaderOptions);
        }

        public async Task<object> Execute()
        {
            var httpReponse = await _httpExecute();
            return httpReponse.Content;
        }

        public async Task<T> BuildAsync(Action<HttpReponseException> ex = null)
        {
            return (await RequestBuildAsync(ex)).Content;
        }

        async Task IHttpConsumerBuilder.BuildAsync(Action<HttpReponseException> ex = null)
        {
            await BuildAsync(ex);
        }

        public async Task<IHttpResponse<T>> BuildWithHeaderAsync(Action<HttpReponseException> ex = null)
        {
            return await RequestBuildAsync(ex);
        }

        async Task<IHttpResponse> IHttpConsumerBuilder.BuildWithHeaderAsync(Action<HttpReponseException> ex = null)
        {
            return await BuildWithHeaderAsync(ex);
        }

        private async Task<IHttpResponse<T>> RequestBuildAsync(Action<HttpReponseException> ex)
        {
            IHttpResponse<T> content = default;
            try
            {
                if (_httpExecute != null)
                {
                    content = await _httpExecute();
                }
                else
                {
                    var consumer = (HttpConsumer)_httpConsumer;

                    List<Task<object>> tasks = new List<Task<object>>();
                    foreach (var execute in consumer.HttpExecute)
                        tasks.Add(execute.Execute());

                    var aggregate = new HttpAggregateResult<T>();
                    foreach (var result in await Task.WhenAll(tasks))
                        aggregate.AddResponse(result);

                    content = new HttpResponse<T>(_aggregate(aggregate), System.Net.HttpStatusCode.OK, null, null);
                    consumer.ClearHttpRequest();
                }
            }
            catch (WebException exception)
            {
                //TODO: Implement circute breaker 
                if (!(ex is null))
                {
                    var responseException = new HttpReponseException(exception.Message, exception);
                    responseException.SetResponse(exception);
                    ex(responseException);
                }
            }
            catch (Exception exception)
            {
                if (!(ex is null))
                {
                    ex(new HttpReponseException(exception.Message, exception));
                }
                else
                {
                    throw;
                }
            }

            return content;
        }
    }
}
