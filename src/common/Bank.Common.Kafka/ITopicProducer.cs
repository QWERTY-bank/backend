using Confluent.Kafka;

namespace Bank.Common.Kafka;

public interface ITopicProducer<TMessage>
{
    Task<DeliveryResult<string, TMessage>> ProduceAsync(string key, TMessage message, CancellationToken cancellationToken);
}