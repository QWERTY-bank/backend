using Bank.Core.Application.Accounts.Requests;
using MediatR;

namespace Bank.Core.Application.Accounts;

public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, long>
{
    public Task<long> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}