using System;

namespace Http.Consumer.Contracts
{
    public interface IHttpAggregateResources 
    {
        IHttpConsumerBuilder<TAggregate> Aggregate<TAggregate>(Action<HttpAggregateResult<TAggregate>, TAggregate> result) where TAggregate : new();
    }
}
