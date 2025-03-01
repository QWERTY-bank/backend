namespace Bank.Core.Domain.Accounts;

public class UnitAccountEntity : AccountBaseEntity
{
    public required Guid UnitId { get; init; }
}