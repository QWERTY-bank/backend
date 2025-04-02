namespace Bank.Common.Kafka.Transfers;

public class TransferResponse
{
    public required Guid Key { get; init; }
    public required TransferStatus Status { get; init; }
}

public enum TransferStatus
{
    Success,
    Fail
}