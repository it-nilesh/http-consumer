using System;
using System.Threading.Tasks;

namespace Http.Consumer
{
    using Http.Consumer.Contracts;
    using Http.Consumer.Exceptions;

    public class HttpConsumerBuilder<T> : IHttpConsumerBuilder<T>, IHttpConsumerBuilder, IHttpConsumerExecute
    {
        public Func<Task<T>> HttpExecute { get; }

        public Func<HttpAggregateResult, T> Aggregate { get; }

        private readonly IHttpConsumer _httpConsumer;

        public HttpConsumerBuilder(Func<Task<T>> httpExecute, IHttpConsumer httpConsumer)
        {
            HttpExecute = httpExecute;
            _httpConsumer = httpConsumer;
        }

        public HttpConsumerBuilder(Func<HttpAggregateResult, T> aggregate, IHttpConsumer httpConsume)
        {
            Aggregate = aggregate;
            _httpConsumer = httpConsume;
        }

        public async Task<T> BuildAsync(Action<HttpReponseException> ex = null)
        {
            T content;
            try
            {
                if (HttpExecute != null)
                {
                    content = await HttpExecute();
                }
                else
                {
                    var httpExecute = ((HttpConsumer)_httpConsumer).HttpExecute;
                    var aggregate = new HttpAggregateResult();
                    foreach (var execute in httpExecute)
                    {
                        aggregate.AddResponse(execute.Execute());
                    }

                    content = Aggregate(aggregate);
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

                content = default(T);
            }

            return content;
        }

        public IHttpAggregateResources Next()
        {
            ((HttpConsumer)_httpConsumer).SetWebRequest(this);
            return new HttpAggregateResources(_httpConsumer);
        }

        public async Task<object> Execute()
        {
            return await HttpExecute();
        }

        async Task IHttpConsumerBuilder.BuildAsync(Action<HttpReponseException> ex = null)
        {
            await BuildAsync(ex);
        }
    }
}
