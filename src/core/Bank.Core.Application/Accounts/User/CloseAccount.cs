using Bank.Core.Domain.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Bank.Core.Application.Accounts.User;

public class CloseAccountCommand : IRequest<OperationResult<Empty>>
{
    public Guid UserId { get; init; }
    public long AccountId { get; init; }
}

public class CloseAccountCommandHandler : IRequestHandler<CloseAccountCommand, OperationResult<Empty>>
{
    private readonly ICoreDbContext _dbContext;

    public CloseAccountCommandHandler(ICoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<OperationResult<Empty>> Handle(CloseAccountCommand request, CancellationToken cancellationToken)
    {
        var account = await _dbContext.PersonalAccounts.SingleOrDefaultAsync(
            account => account.Id == request.AccountId && account.UserId == request.UserId,
            cancellationToken);

        if (account == null)
        {
            return OperationResultFactory.NotFound<Empty>(request.AccountId);
        }
        
        account.IsClosed = true;
        
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return OperationResultFactory.EmptyResult;
    }
}