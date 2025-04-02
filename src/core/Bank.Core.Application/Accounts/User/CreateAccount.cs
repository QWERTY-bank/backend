using Bank.Core.Domain.Accounts;
using Bank.Core.Domain.Common;
using Bank.Core.Domain.Currencies;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Bank.Core.Application.Accounts.User;

public class CreateAccountCommand : IRequest<OperationResult<long>>
{
    public required Guid UserId { get; init; }
    public required string Title { get; init; }
    public required CurrencyCode Code { get; init; }
}

public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, OperationResult<long>>
{
    private readonly ICoreDbContext _dbContext;

    public CreateAccountCommandHandler(ICoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<OperationResult<long>> Handle(CreateAccountCommand command, CancellationToken cancellationToken)
    {
        var account = new PersonalAccountEntity
        {
            Title = command.Title,
            UserId = command.UserId,
            AccountCurrencies =
            [
                new AccountCurrencyEntity
                {
                    Code = command.Code,
                    Value = 0
                }
            ]
        };
        
        _dbContext.Accounts.Add(account);

        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException e)
        {
            var nameIsNotUnique = e.InnerException is PostgresException
            {
                ConstraintName: "ix_account_entity_user_id_title"
            };

            if (nameIsNotUnique)
            {
                return OperationResultFactory.NameIsNotUnique<long>(command.Title, "Счет");
            }
            
            throw;
        }

        return OperationResultFactory.Success(account.Id);
    }
}