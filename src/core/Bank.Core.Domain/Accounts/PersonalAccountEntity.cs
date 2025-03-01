namespace Bank.Core.Domain.Accounts;

public class PersonalAccountEntity : AccountBaseEntity
{
    public required Guid UserId { get; init; }
}