using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Confluent.Kafka.Extensions.Diagnostics;

namespace Bank.Common.Kafka.Internal;

internal class KafkaConsumer<TMessage> : IKafkaConsumer  
{
    private readonly string _topicName;
    private readonly ConsumerConfig _consumerConfig;
    private readonly IServiceProvider _serviceProvider;

    public KafkaConsumer(
        string topicName, 
        ConsumerConfig consumerConfig,
        IServiceProvider serviceProvider)
    {
        _topicName = topicName;
        _consumerConfig = consumerConfig;
        _serviceProvider = serviceProvider;
    }

    public async Task StartConsume(CancellationToken cancellationToken)
    {
        using var consumer = new ConsumerBuilder<string, TMessage>(_consumerConfig)
            .SetValueDeserializer(new KafkaJsonDeserializer<TMessage>())
            .Build();
        consumer.Subscribe(_topicName);

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                await consumer.ConsumeWithInstrumentation(async (result, _) =>
                {
                    using var scope = _serviceProvider.CreateScope();

                    var topicConsumer = scope.ServiceProvider.GetRequiredService<ITopicConsumer<TMessage>>();

                    await topicConsumer.ConsumeAsync(result!, cancellationToken);

                    consumer.Commit(result);
                }, cancellationToken);

                var consumerResult = consumer.Consume();
            }
            catch (Exception)
            {
                // печально, будет ретрай
            }
        }
    }
}