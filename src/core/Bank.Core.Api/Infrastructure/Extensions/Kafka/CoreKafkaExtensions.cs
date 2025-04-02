using Bank.Common.Kafka;
using Bank.Common.Kafka.Transfers;
using Bank.Core.Kafka.Transfers;

namespace Bank.Core.Api.Infrastructure.Extensions.Kafka;

public static class CoreKafkaExtensions
{
    public static IServiceCollection AddCoreKafka(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var kafkaSettings = configuration.GetSection("KafkaSettings").Get<KafkaSettings>()!;

        services.AddConsumer<UnitAccountTransferModel, TransferConsumer>(
            kafkaSettings.BootstrapServers,
            kafkaSettings.TransferConsumer.TopicName,
            kafkaSettings.TransferConsumer.ConsumerGroup);
 
        services.AddProducer<TransferResponse>(
            kafkaSettings.BootstrapServers,
            kafkaSettings.TransferResponseProducer.TopicName);
        
        services.AddProducer<UnitAccountTransferModel>(
            kafkaSettings.BootstrapServers,
            kafkaSettings.TransferConsumer.TopicName);

        return services;
    }
}