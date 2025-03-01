namespace Bank.Core.Application.Common;

/// <summary>
/// Период дат
/// </summary>
public class Period
{
    /// <summary>
    /// Начало
    /// </summary>
    public DateTimeOffset Start { get; init; } = DateTimeOffset.MinValue;

    /// <summary>
    /// Конец
    /// </summary>
    public DateTimeOffset End { get; init; } = DateTimeOffset.MaxValue;

    public static Period All => new() {Start = DateTimeOffset.MinValue, End = DateTimeOffset.MaxValue};
}
