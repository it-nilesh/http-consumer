
namespace Http.Consumer
{
    using Http.Consumer.Contracts;
    using System;

    public class HttpAggregateResources : IHttpAggregateResources
    {
        private readonly IHttpConsumer _httpConsumer;

        public HttpAggregateResources(IHttpConsumer httpConsumer)
        {
            _httpConsumer = httpConsumer;
        }

        public IHttpConsumerBuilder<TAggregate> Aggregate<TAggregate>(Action<HttpAggregateResult, TAggregate> result) where TAggregate : new()
        {
            TAggregate resultFunc(HttpAggregateResult @obj)
            {
                TAggregate aggregate = new TAggregate();
                result(@obj, aggregate);
                return aggregate;
            }

            return new HttpConsumerBuilder<TAggregate>(resultFunc, _httpConsumer);
        }
    }
}
