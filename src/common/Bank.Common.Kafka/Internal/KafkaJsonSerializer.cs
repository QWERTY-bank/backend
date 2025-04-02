using System.Text.Json;
using Confluent.Kafka;

namespace Bank.Common.Kafka.Internal;

internal class KafkaJsonSerializer<TMessage> : ISerializer<TMessage>
{
    public byte[] Serialize(TMessage data, SerializationContext context)
    {
        return JsonSerializer.SerializeToUtf8Bytes(data);
    }
}