using MediatR;

namespace Bank.Core.Application.Accounts.Requests;

public class CreateAccountCommand : IRequest<long>
{
    public Guid AccountId { get; set; }
}