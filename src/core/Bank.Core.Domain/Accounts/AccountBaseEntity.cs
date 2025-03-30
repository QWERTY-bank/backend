using Bank.Core.Domain.Common;
using Bank.Core.Domain.Transactions;

namespace Bank.Core.Domain.Accounts;

public abstract class AccountBaseEntity : BaseEntity<long>
{
    private const int Decimals = 2;
    
    public required string Title { get; init; }
    public bool IsClosed { get; set; } = false;
    public List<TransactionEntity> Transactions { get; init; } = [];
    public List<AccountCurrencyEntity> AccountCurrencies { get; init; } = [];

    public OperationResult<Empty> Deposit(DepositTransactionEntity transaction)
    {
        var currentCurrencies = AccountCurrencies.ToDictionary(value => value.Code);
        
        foreach (var newCurrency in transaction.Currencies)
        {
            if (newCurrency.Value <= 0)
            {
                return OperationResultFactory.InvalidData<Empty>("Значение валюты должно быть положительным");
            }
            
            if (currentCurrencies.TryGetValue(newCurrency.Code, out var currency))
            {
                currency.Value += Math.Round(newCurrency.Value, Decimals);
            }
            else
            {
                return OperationResultFactory.InvalidData<Empty>($"На счете отсутствует валюта {newCurrency.Code}");
            }
        }
        
        Transactions.Add(transaction);
            
        return OperationResultFactory.EmptyResult;
    } 
    
    public OperationResult<Empty> Withdraw(WithdrawTransactionEntity transaction)
    {
        var currentCurrencies = AccountCurrencies.ToDictionary(value => value.Code);
        
        foreach (var newCurrency in transaction.Currencies)
        {
            if (newCurrency.Value <= 0)
            {
                return OperationResultFactory.InvalidData<Empty>(
                    $"Значение валюты {newCurrency.Code} должно быть положительным");
            }

            var hasCurrency = currentCurrencies.TryGetValue(newCurrency.Code, out var currency);

            if (!hasCurrency)
            {
                return OperationResultFactory.InvalidData<Empty>($"На счете отсутствует валюта {newCurrency.Code}");
            }

            if (currency!.Value - newCurrency.Value < 0)
            {
                return OperationResultFactory.InvalidData<Empty>(
                    $"На счете не достаточно валюты {newCurrency.Code} для списания");
            }
            
            currency.Value -= Math.Round(newCurrency.Value, Decimals);
        }
        
        Transactions.Add(transaction);
            
        return OperationResultFactory.EmptyResult;
    } 
}