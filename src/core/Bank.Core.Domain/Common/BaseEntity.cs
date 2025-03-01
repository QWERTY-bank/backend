namespace Bank.Core.Domain.Common;

/// <summary>
/// Сущность
/// </summary>
public abstract class BaseEntity<TId> where TId : struct
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public TId Id { get; protected set; }
}