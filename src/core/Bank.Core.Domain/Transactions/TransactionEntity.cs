using System.Transactions;
using Bank.Core.Domain.Common;

namespace Bank.Core.Domain.Transactions;

/// <summary>
/// Транзакция
/// </summary>
public abstract class TransactionEntity : BaseEntity<long>
{
    /// <summary>
    /// Уникальный идентификатор транзакции
    /// </summary>
    public required Guid Key { get; init; }
    
    public TransactionType Type { get; init; }
    
    /// <summary>
    /// Ключ родительской транзакции
    /// </summary>
    public Guid? ParentKey { get; init; }
    
    /// <summary>
    /// Дата сохранения транзакции в хранилище
    /// </summary>
    public required DateTime CreatedDate { get; init; }

    /// <summary>
    /// Операционная дата транзакции
    /// </summary>
    public required DateTime OperationDate { get; init; }

    /// <summary>
    /// Отменена ли транзакция
    /// </summary>
    public bool IsCancel { get; init; }
    
    public required long AccountId { get; init; }

    /// <summary>
    /// Дочерние транзакции
    /// </summary>
    public List<TransactionEntity> ChildTransactions = [];
    
    public List<TransactionCurrency> Currencies = [];
}