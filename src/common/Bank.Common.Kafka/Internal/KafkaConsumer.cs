using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;

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
                var consumerResult = consumer.Consume();

                using var scope = _serviceProvider.CreateScope();
                
                var topicConsumer = scope.ServiceProvider.GetRequiredService<ITopicConsumer<TMessage>>();

                await topicConsumer.ConsumeAsync(consumerResult, cancellationToken);

                consumer.Commit(consumerResult);
            }
            catch (Exception)
            {
                // печально, будет ретрай
            }
        }
    }
}