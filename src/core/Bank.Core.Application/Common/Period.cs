namespace Bank.Core.Application.Common;

/// <summary>
/// Период дат
/// </summary>
public class Period
{
    /// <summary>
    /// Начало
    /// </summary>
    public required DateTimeOffset Start { get; init; }

    /// <summary>
    /// Конец
    /// </summary>
    public required DateTimeOffset End { get; init; }

    public static Period All => new() {Start = DateTimeOffset.MinValue, End = DateTimeOffset.MaxValue};
}
