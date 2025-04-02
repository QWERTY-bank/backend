using Microsoft.Extensions.Hosting;

namespace Bank.Common.Kafka.Internal;

internal class KafkaConsumerBackgroundService<TKafkaConsumer> : BackgroundService where TKafkaConsumer : IKafkaConsumer
{
    private readonly TKafkaConsumer _kafkaConsumer;
    
    public KafkaConsumerBackgroundService(TKafkaConsumer kafkaConsumer)
    {
        _kafkaConsumer = kafkaConsumer;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.Factory.StartNew((Func<Task>) (() => _kafkaConsumer.StartConsume(stoppingToken)), CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
    }
}