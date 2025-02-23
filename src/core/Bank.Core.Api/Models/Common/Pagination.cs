namespace Bank.Core.Api.Models.Common;

/// <summary>
/// Пагинация
/// </summary>
public class Pagination
{
    public required int Skip { get; init; }
    
    public required int Take { get; init; }
}