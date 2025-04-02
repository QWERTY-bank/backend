using Confluent.Kafka;

namespace Bank.Common.Kafka.Internal;

internal class KafkaProducer<TMessage> : ITopicProducer<TMessage>, IDisposable
{
    private readonly string _topicName;
    private readonly IProducer<string, TMessage> _producer;

    public KafkaProducer(
        ProducerConfig producerConfig,
        string topicName)
    {
        _topicName = topicName;

        _producer = new ProducerBuilder<string, TMessage>(producerConfig)
            .SetValueSerializer(new KafkaJsonSerializer<TMessage>())
            .Build();
    }

    public Task<DeliveryResult<string, TMessage>> ProduceAsync(string key, TMessage message, CancellationToken cancellationToken)
    {
        var kafkaMessage = new Message<string, TMessage>
        {
            Key = key,
            Value = message
        };

        return _producer.ProduceAsync(_topicName, kafkaMessage, cancellationToken);
    }

    public void Dispose()
    {
        _producer.Dispose();
    }
}