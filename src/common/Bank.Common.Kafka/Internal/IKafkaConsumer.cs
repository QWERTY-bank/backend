namespace Bank.Common.Kafka.Internal;

internal interface IKafkaConsumer
{
    Task StartConsume(CancellationToken cancellationToken);
}