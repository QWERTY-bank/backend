namespace Bank.Core.Domain.Accounts;

public enum AccountType
{
    /// <summary>
    /// Личный кошелек.
    /// </summary>
    Personal = 0,
    
    /// <summary>
    /// Кошелек юнита.
    /// </summary>
    Unit = 1,
}