using Confluent.Kafka;

namespace Bank.Common.Kafka;

public interface ITopicConsumer<TMessage>
{
    Task ConsumeAsync(ConsumeResult<string, TMessage> message, CancellationToken cancellationToken);
}