namespace Bank.Core.Api.Infrastructure.Extensions.Kafka;

public class KafkaSettings
{
    public required string BootstrapServers { get; init; }
    public required KafkaConsumer TransferConsumer { get; init; }
    public required KafkaProducer TransferResponseProducer { get; init; }
}

public class KafkaConsumer
{
    public required string TopicName { get; init; }
    public required string ConsumerGroup { get; init; }
}

public class KafkaProducer
{
    public required string TopicName { get; init; }
}