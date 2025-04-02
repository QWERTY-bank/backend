using Bank.Common.Kafka.Internal;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;

namespace Bank.Common.Kafka;

public static class KafkaExtensions
{
    public static void AddConsumer<TMessage, TConsumer>(
        this IServiceCollection services,
        string bootstrapServers,
        string topicName,
        string consumerGroup)
        where TConsumer : class, ITopicConsumer<TMessage> 
        where TMessage : class
    {
        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = bootstrapServers,
            AutoOffsetReset = AutoOffsetReset.Earliest,
            GroupId = consumerGroup,
            EnableAutoCommit = false,
            SecurityProtocol = SecurityProtocol.Plaintext
        };

        services.AddScoped<ITopicConsumer<TMessage>, TConsumer>();
        services.AddSingleton<KafkaConsumer<TMessage>>(sp =>
            new KafkaConsumer<TMessage>(topicName, consumerConfig, sp));
        services.AddHostedService<KafkaConsumerBackgroundService<KafkaConsumer<TMessage>>>();
    }
    
    public static void AddProducer<TMessage>(
        this IServiceCollection services,
        string bootstrapServers,
        string topicName)
        where TMessage : class
    {
        var producerConfig = new ProducerConfig()
        {
            BootstrapServers = bootstrapServers,
            AllowAutoCreateTopics = true,
            Acks = Acks.All,
            SecurityProtocol = SecurityProtocol.Plaintext
        };
        
        services.AddTransient<ITopicProducer<TMessage>>(_ =>
            new KafkaProducer<TMessage>(producerConfig, topicName));
    }
}